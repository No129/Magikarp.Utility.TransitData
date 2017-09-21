using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Magikarp.Utility.TransitData
{
    public class DefaultConverter<TModel> : BaseConverter where TModel : new()
    {

        #region -- 變數宣告 ( Declarations ) --   

        private TransitDataAdapter l_objAdapter = null;

        #endregion

        #region -- 建構/解構 ( Constructors/Destructor ) --

        /// <summary>
        /// 建構元。
        /// </summary>
        /// <param name="pi_objTransitAdapter">資料轉接器。</param>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public DefaultConverter(TransitDataAdapter pi_objTransitAdapter)
        {
            this.l_objAdapter = pi_objTransitAdapter;
        }

        #endregion
             
        #region -- 方法 ( Public Method ) --

        /// <summary>
        /// 設定資料物件屬性值。
        /// </summary>
        /// <param name="pi_objContainer">資料物件。</param>
        /// <param name="pi_objProperty">待設定屬性物件。</param>
        /// <param name="pi_objValue">待設定內容。</param>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public override void SetValue(object pi_objContainer, PropertyInfo pi_objProperty, XElement pi_objValue)
        {          

            TModel objValue = new TModel();
            IDataConverter<XElement> objConverter = this.l_objAdapter.GetConverter(pi_objProperty);

            foreach(System.Reflection.PropertyInfo objProperty in objValue.GetType().GetProperties())
            {
                if(objProperty.Name == pi_objProperty.Name)
                {
                    objConverter.Successor = new EmptyConverter();
                    objConverter.SetValue(pi_objContainer, pi_objProperty, objConverter.Export(objValue, objProperty));
                    break;
                }
            }
        }

        #endregion
    }
}
