using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Magikarp.Utility.TransitData
{

    /// <summary>
    /// 提供中介資料(包含「String」與「XElement」型態)與資料物件的轉換功能。
    /// </summary>
    /// <remarks>
    /// Author: 黃竣祥
    /// Version: 20171020
    /// </remarks>
    public class TransitDataAdapter : ITransitDataAdapter<string>, ITransitDataAdapter<XElement>, ITransitDataAdapter<object>
    {

        #region -- 變數宣告 ( Declarations ) --   

        private Dictionary<String, IDataConverter<XElement>> l_objPropertyConverters = new Dictionary<String, IDataConverter<XElement>>();

        #endregion

        #region -- 建構/解構 ( Constructors/Destructor ) --

        /// <summary>
        /// 建構元。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/10/20
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public TransitDataAdapter()
        {
            this.l_objPropertyConverters.Add("String", new StringConverter());
            this.l_objPropertyConverters.Add("Int64", new LongConverter());
            this.l_objPropertyConverters.Add("Int32", new IntegerConverter());
            this.l_objPropertyConverters.Add("Int16", new ShortConverter());
            this.l_objPropertyConverters.Add("Double", new DoubleConverter());
            this.l_objPropertyConverters.Add("Single", new SingleConverter());
            this.l_objPropertyConverters.Add("Boolean", new BooleanConverter());
            this.l_objPropertyConverters.Add("DateTime", new DateConverter());
            this.l_objPropertyConverters.Add("XDocument", new XDocumentConverter());
            this.l_objPropertyConverters.Add("Enum", new EnumConverter());
            this.l_objPropertyConverters.Add("IList", new ListConverter());
        }

        #endregion     

        #region -- 方法 ( Public Method ) --

        /// <summary>
        /// 取得要求對應的轉換器。
        /// </summary>
        /// <param name="pi_objProperty">建立對應轉換器所需資訊。</param>
        /// <returns>指定型別的轉換器。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        internal IDataConverter<XElement> GetConverter(System.Reflection.PropertyInfo pi_objProperty)
        {
            return this.FindConverter(pi_objProperty);
        }

        #endregion

        #region -- 介面實做 ( Implements ) - [ITransitDataAdapter<string>] --

        /// <summary>
        /// 載入中介資料並依指定的資料物件回傳。
        /// </summary>
        /// <typeparam name="TModel">指定回傳的資料物件型別。</typeparam>
        /// <param name="pi_objSource">待載入的中介資料。</param>
        /// <returns>載入中介資料的資料物件。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public TModel Loading<TModel>(string pi_sSource) where TModel : new()
        {
            TModel objReturn = default(TModel);
            XElement objSource = null;

            int nStep = 0;//程序進度指標。
            Boolean bRun = true;//程序中斷旗標。

            while (bRun)
            {
                nStep += 1;
                switch (nStep)
                {
                    case 1:// 解析傳入的字串。
                        try
                        {
                            objSource = XElement.Parse(pi_sSource);
                        }
                        catch
                        {
                            objReturn = new TModel();
                            bRun = false;
                        }
                        break;
                    case 2:// 透過 XElement 進行載入。
                        objReturn = this.Loading<TModel>(objSource);
                        break;
                    default://結束。
                        bRun = false;
                        break;
                }
            }

            return objReturn;
        }

        /// <summary>
        /// 匯出資料物件為中介資料型態。
        /// </summary>
        /// <typeparam name="TModel">待匯出資料物件型別。</typeparam>
        /// <param name="pi_objSource">待匯出資料的資料物件。</param>
        /// <returns>匯出資料物件的中介資料。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public string Export<TModel>(TModel pi_objSource)
        {
            return this.ExportToXElement<TModel>(pi_objSource).ToString();
        }

        #endregion

        #region -- 介面實做 ( Implements ) - [ITransitDataAdapter<XElement>] --

        /// <summary>
        /// 載入中介資料並依指定的資料物件回傳。
        /// </summary>
        /// <typeparam name="TModel">指定回傳的資料物件型別。</typeparam>
        /// <param name="pi_objSource">待載入的中介資料。</param>
        /// <returns>載入中介資料的資料物件。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public TModel Loading<TModel>(XElement pi_objSource) where TModel : new()
        {
            TModel objReturn = new TModel();
            DefaultConverter<TModel> objDefaultConverter = new DefaultConverter<TModel>(this);

            if (pi_objSource != null)
            {
                foreach (System.Reflection.PropertyInfo objProperty in objReturn.GetType().GetProperties())
                {
                    if (objProperty.CanWrite && this.IsExistPropertyValues(pi_objSource, objProperty))
                    {
                        string sTagName = string.Empty;
                        TransitDataTagAttribute[] objTags = (TransitDataTagAttribute[])objProperty.GetCustomAttributes(typeof(TransitDataTagAttribute), false);
                        IDataConverter<XElement> objConverter = this.FindConverter(objProperty);

                        if ((objTags != null) && (objTags.Count() > 0))
                        {
                            sTagName = objTags[0].ImportTag;
                        }

                        if (string.IsNullOrEmpty(sTagName)) { sTagName = objProperty.Name; }

                        objConverter.Successor = objDefaultConverter;
                        objConverter.SetValue(objReturn, objProperty, pi_objSource.Element(sTagName));
                    }
                }
            }
            return objReturn;
        }

        /// <summary>
        /// 匯出資料物件為中介資料型態。
        /// </summary>
        /// <typeparam name="TModel">待匯出資料物件型別。</typeparam>
        /// <param name="pi_objSource">待匯出資料的資料物件。</param>
        /// <returns>匯出資料物件的中介資料。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        XElement ITransitDataAdapter<XElement>.Export<TModel>(TModel pi_objSource)
        {
            return this.ExportToXElement(pi_objSource);
        }

        #endregion

        #region -- 介面實做 ( Implements ) - [ITransitDataAdapter<object>] --

        /// <summary>
        /// 載入中介資料並依指定的資料物件回傳。
        /// </summary>
        /// <typeparam name="TModel">指定回傳的資料物件型別。</typeparam>
        /// <param name="pi_objSource">待載入的中介資料。</param>
        /// <returns>載入中介資料的資料物件。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public TModel Loading<TModel>(object pi_objSource) where TModel : new()
        {
            TModel objReturn = default(TModel);

            if (pi_objSource.GetType() == typeof(string))
            {
                ITransitDataAdapter<string> objAdapter = new TransitDataAdapter();

                objReturn = objAdapter.Loading<TModel>((string)pi_objSource);
            }
            else if (pi_objSource.GetType() == typeof(XElement))
            {
                ITransitDataAdapter<XElement> objAdapter = new TransitDataAdapter();

                objReturn = objAdapter.Loading<TModel>((XElement)pi_objSource);
            }
            else
            {
                objReturn = new TModel();
            }

            return objReturn;
        }

        /// <summary>
        /// 匯出資料物件為中介資料型態。
        /// </summary>
        /// <typeparam name="TModel">待匯出資料物件型別。</typeparam>
        /// <param name="pi_objSource">待匯出資料的資料物件。</param>
        /// <returns>匯出資料物件的中介資料。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        object ITransitDataAdapter<object>.Export<TModel>(TModel pi_objSource)
        {
            return this.ExportToXElement<TModel>(pi_objSource).ToString();
        }

        #endregion

        #region -- 私有函式 ( Private Method) --

        /// <summary>
        /// 確認來源資料是否存在對應屬性。(True:存在／False:缺少)
        /// </summary>
        /// <param name="pi_objSource">來源中介資料。</param>
        /// <param name="pi_objProperty">指定的屬性物件。</param>
        /// <returns>來源資料是否存在對應屬性。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        private Boolean IsExistPropertyValues(XElement pi_objSource, System.Reflection.PropertyInfo pi_objProperty)
        {
            string sPropertyName = pi_objProperty.Name;
            IEnumerable<XElement> objQuery =
                from XElement objElement in pi_objSource.Elements()
                where objElement.Name.LocalName.EndsWith(sPropertyName)
                select objElement;

            return objQuery.Any();
        }

        /// <summary>
        /// 取得指定資料型別的轉換器。
        /// </summary>
        /// <param name="pi_objProperty">指定資料型別。</param>
        /// <returns>指定資料型別的轉換器。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: 
        ///     加上 List 處理。 (黃竣祥 2017/10/20)
        /// DB Object: N/A      
        /// </remarks>
        private IDataConverter<XElement> FindConverter(System.Reflection.PropertyInfo pi_objProperty)
        {
            IDataConverter<XElement> objReturn = null;

            if (pi_objProperty.PropertyType.IsEnum)
            {
                objReturn = this.l_objPropertyConverters["Enum"];
            }
            else if (this.l_objPropertyConverters.ContainsKey(pi_objProperty.PropertyType.Name))
            {
                objReturn = this.l_objPropertyConverters[pi_objProperty.PropertyType.Name];

            }
            else if (pi_objProperty.PropertyType.IsGenericType && pi_objProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                objReturn = this.l_objPropertyConverters["IList"];
            }

            return objReturn;
        }

        /// <summary>
        /// 匯出傳入物件為 XElement 資料型態。
        /// </summary>
        /// <typeparam name="TModel">待匯出資料物件型別。</typeparam>
        /// <param name="pi_objSource">待匯出資料的資料物件。</param>
        /// <returns>匯出資料物件的中介資料。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        private XElement ExportToXElement<TModel>(TModel pi_objSource)
        {
            XElement objRoot = new XElement("Root");
            EmptyConverter objEmptyConverter = new EmptyConverter();

            foreach (System.Reflection.PropertyInfo objProperty in pi_objSource.GetType().GetProperties())
            {
                if (objProperty.CanRead)
                {
                    XElement objChild = null;
                    IDataConverter<XElement> objConverter = this.FindConverter(objProperty);

                    objConverter.Successor = objEmptyConverter;
                    objChild = objConverter.Export(pi_objSource, objProperty);
                    if (objChild != null)
                    {
                        objRoot.Add(objChild);
                    }
                }
            }
            return objRoot;
        }

        #endregion

    }
}
