using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magikarp.Utility.TransitData
{

    /// <summary>
    /// 提供中介資料轉換時自訂中介資料的節點名稱。
    /// </summary>
    /// <remarks>
    /// Author: 黃竣祥
    /// Version: 20170921
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class TransitDataTagAttribute : System.Attribute
    {

        #region -- 建構/解構 ( Constructors/Destructor ) --

        /// <summary>
        /// 建構元。
        /// </summary>
        /// <param name="pi_sImportTag">匯入資料的節點名稱。</param>
        /// <param name="pi_sOutputTag">匯出資料的節點名稱。</param>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public TransitDataTagAttribute(string pi_sImportTag, string pi_sOutputTag)
        {
            this.ImportTag = pi_sImportTag;
            this.OutputTag = pi_sOutputTag;
        }
        #endregion

        #region -- 屬性 ( Properties ) --              

        /// <summary>
        /// 設定或取得匯入資料的節點名稱。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public string ImportTag { get; set; }

        /// <summary>
        /// 設定或取得匯出資料的節點名稱。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public string OutputTag { get; set; }

        #endregion

    }
}
