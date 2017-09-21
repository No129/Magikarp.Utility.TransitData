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
    /// 提供 Integer 型別資料轉換功能。
    /// </summary>
    /// <remarks>
    /// Author: 黃竣祥
    /// Version: 20170921
    /// </remarks>
    public class IntegerConverter : BaseConverter
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
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public override void SetValue(object pi_objContainer, PropertyInfo pi_objProperty, XElement pi_objValue)
        {
            if ((pi_objValue != null) && (int.TryParse(pi_objValue.Value, out int nValue)))
            {
                pi_objProperty.SetValue(pi_objContainer, nValue, null);
            }
            else
            {
                base.Successor.SetValue(pi_objContainer, pi_objProperty, pi_objValue);
            }
        }

        #endregion
    }
}
