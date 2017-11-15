using System;
using System.Collections;
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
    /// Version: 20171115
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
        /// History: 
        ///     參考 https://goo.gl/eXCfHh 解說，調整增加項目方式。 (黃竣祥 2017/10/20)
        /// DB Object: N/A      
        /// </remarks>
        public override void SetValue(object pi_objContainer, PropertyInfo pi_objProperty, XElement pi_objValue)
        {
            string sType = pi_objValue.Attribute("Type").Value;
            var listType = typeof(List<>);
            var genericArgs = pi_objProperty.PropertyType.GetGenericArguments();
            var concreteType = listType.MakeGenericType(genericArgs);
            var newList = Activator.CreateInstance(concreteType);
            var addMethod = concreteType.GetMethod("Add");

            switch (Type.GetTypeCode(genericArgs[0]))
            {
                case TypeCode.String:
                    foreach (XElement objElement in pi_objValue.Elements())
                    {
                        addMethod.Invoke(newList, new object[] { objElement.Value });
                    }
                    break;

                case TypeCode.Boolean:
                    foreach (XElement objElement in pi_objValue.Elements())
                    {
                        addMethod.Invoke(newList, new object[] { Boolean.Parse(objElement.Value) });
                    }
                    break;

                default:
                    switch (sType)
                    {
                        case "System.Xml.Linq.XElement":
                            foreach (XElement objElement in pi_objValue.Elements())
                            {
                                addMethod.Invoke(newList, new object[] { XElement.Parse(objElement.Value) });
                            }
                            break;
                    }
                    break;
            }

            pi_objProperty.SetValue(pi_objContainer, newList);
        }

        /// <summary>
        /// 匯出中介資料。
        /// </summary>
        /// <param name="pi_objContainer">資料物件。</param>
        /// <param name="pi_objProperty">待匯出屬性物件。</param>
        /// <returns>指定型別的中介資料。</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term>Author:</term><description>黃竣祥</description></item>        
        /// <item><term>History</term><description>
        /// <list type="number">
        /// <item><term>2017/10/20</term><description>建立方法。(黃竣祥）</description></item>
        /// <item><term>2017/10/20</term><description>調整設值方式並新增 Boolean 項目。(黃竣祥）</description></item>
        /// <item><term>2017/11/15</term><description>增加檢查以避免缺少物件錯誤。(黃竣祥）</description></item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
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

                        switch (Type.GetTypeCode(tArg))
                        {
                            case TypeCode.String:
                                List<string> objStringList = (List<string>)pi_objProperty.GetValue(pi_objContainer, System.Reflection.BindingFlags.Default, null, null, null);

                                objReturn = new XElement(sTagName, new XAttribute("Type", "String"));

                                foreach (string sItem in objStringList)
                                {
                                    objReturn.Add(new XElement("Item", sItem));
                                }
                                break;

                            case TypeCode.Boolean:
                                List<Boolean> objBooleanList = (List<Boolean>)pi_objProperty.GetValue(pi_objContainer, System.Reflection.BindingFlags.Default, null, null, null);

                                objReturn = new XElement(sTagName, new XAttribute("Type", "Boolean"));
                                foreach (Boolean sItem in objBooleanList)
                                {
                                    objReturn.Add(new XElement("Item", sItem.ToString()));
                                }
                                break;

                            default:
                                switch (tArg.FullName.ToString())
                                {
                                    case "System.Xml.Linq.XElement":
                                        List<XElement> objXElementList = (List<XElement>)pi_objProperty.GetValue(pi_objContainer, System.Reflection.BindingFlags.Default, null, null, null);

                                        objReturn = new XElement(sTagName, new XAttribute("Type", "System.Xml.Linq.XElement"));
                                        if(objXElementList != null)
                                        {
                                            foreach (XElement objItem in objXElementList)
                                            {
                                                objReturn.Add(new XElement("Item", new XCData(objItem.ToString())));
                                            }
                                        }                                       
                                        break;
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
