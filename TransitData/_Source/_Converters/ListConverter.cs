using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Magikarp.Utility.TransitData
{
    /// <summary>
    /// 提供 IList 型別資料轉換功能。
    /// </summary>
    /// <remarks>
    /// Author: 黃竣祥
    /// Version: 20171020
    /// </remarks>
    public class ListConverter : BaseConverter
    {
        #region -- 方法 ( Public Method ) --

        /// <summary>
        /// 設定資料物件屬性值。
        /// </summary>
        /// <param name="pi_objContainer">資料物件。</param>
        /// <param name="pi_objProperty">待設定屬性物件。</param>
        /// <param name="pi_objValue">待設定內容。</param>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/10/20
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public override void SetValue(object pi_objContainer, PropertyInfo pi_objProperty, XElement pi_objValue)
        {
            string sType = pi_objValue.Attribute("Type").Value;

            switch (sType)
            {
                case "System.String":
                    List<string> objList = new List<string>();

                    foreach (XElement objElement in pi_objValue.Elements())
                    {
                        objList.Add(objElement.Value);
                    }
                    pi_objProperty.SetValue(pi_objContainer, objList, null);
                    break;

                case "System.Xml.Linq.XElement":
                    List<XElement> objXElementList = new List<XElement>();

                    foreach (XElement objElement in pi_objValue.Elements())
                    {
                        objXElementList.Add(XElement.Parse(objElement.Value));
                    }
                    pi_objProperty.SetValue(pi_objContainer, objXElementList, null);
                    break;
            }
        }

        /// <summary>
        /// 匯出中介資料。
        /// </summary>
        /// <param name="pi_objContainer">資料物件。</param>
        /// <param name="pi_objProperty">待匯出屬性物件。</param>
        /// <returns>指定型別的中介資料。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/10/20
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public override XElement Export(object pi_objContainer, PropertyInfo pi_objProperty)
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
                        Type tArg = pi_objProperty.PropertyType.GetGenericArguments()[0];

                        switch (tArg.FullName.ToString())
                        {
                            case "System.String":
                                List<string> objList = (List<string>)pi_objProperty.GetValue(pi_objContainer, System.Reflection.BindingFlags.Default, null, null, null);

                                objReturn = new XElement(sTagName, new XAttribute("Type", "System.String"));
                                foreach (string sItem in objList)
                                {
                                    objReturn.Add(new XElement("Item", sItem));
                                }
                                break;

                            case "System.Xml.Linq.XElement":
                                List<XElement> objXElementList = (List<XElement>)pi_objProperty.GetValue(pi_objContainer, System.Reflection.BindingFlags.Default, null, null, null);

                                objReturn = new XElement(sTagName, new XAttribute("Type", "System.Xml.Linq.XElement"));
                                foreach (XElement objItem in objXElementList)
                                {
                                    objReturn.Add(new XElement("Item", new XCData(objItem.ToString())));
                                }
                                break;
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
    }
}
