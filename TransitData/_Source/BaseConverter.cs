using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Magikarp.Utility.TransitData
{

    /// <summary>
    /// 提供 Converter 共用功能。
    /// </summary>
    /// <remarks>
    /// Author: 黃竣祥
    /// Version: 20170921
    /// </remarks>
    public abstract class BaseConverter : IDataConverter<XElement>
    {

        #region -- 方法 ( Public Method ) --

        /// <summary>
        /// 匯出中介資料。
        /// </summary>
        /// <param name="pi_objContainer">資料物件。</param>
        /// <param name="pi_objProperty">待匯出屬性物件。</param>
        /// <returns>指定型別的中介資料。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        virtual public XElement Export(object pi_objContainer, PropertyInfo pi_objProperty)
        {
            XElement objReturn = null;
            string sTagName = string.Empty;

            int nStep = 0;          // 程序進度指標。
            Boolean bRun = true;    // 程序中斷旗標。

            while (bRun)
            {
                nStep += 1;
                switch (nStep)
                {
                    case 1:// 取得 Tag 名稱。
                        TransitDataTagAttribute[] objTags = (TransitDataTagAttribute[])pi_objProperty.GetCustomAttributes(typeof(TransitDataTagAttribute), false);

                        if (objTags != null && objTags.Count() > 0)
                        {
                            sTagName = objTags[0].OutputTag;
                        }

                        if (string.IsNullOrEmpty(sTagName))
                        {
                            sTagName = pi_objProperty.Name;
                        }

                        break;

                    case 2:// 填入屬性值。
                        if (pi_objProperty.GetValue(pi_objContainer, System.Reflection.BindingFlags.Default, null, null, null) != null)
                        {
                            objReturn = new XElement(sTagName,
                                pi_objProperty.GetValue(pi_objContainer, System.Reflection.BindingFlags.Default, null, null, null).ToString());
                        }
                        else
                        {
                            objReturn = this.Successor.Export(pi_objContainer, pi_objProperty);
                        }

                        break;
                    default:// 結束。
                        bRun = false;
                        break;
                }
            }

            return objReturn;
        }

        #endregion

        #region -- 屬性 ( Properties ) --

        /// <summary>
        /// 取得或設定後續處理實體。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public IDataConverter<XElement> Successor { get; set; }

        #endregion

        #region -- 抽象函式 ( Abstract Method ) --  

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
        public abstract void SetValue(object pi_objContainer, PropertyInfo pi_objProperty, XElement pi_objValue);

        #endregion
    }
}
