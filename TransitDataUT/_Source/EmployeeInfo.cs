using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TransitDataUT
{
    /// <summary>
    /// 定義僱員資料。
    /// </summary>
    /// <remarks>
    /// Author: 黃竣祥
    /// Version: 20170921
    /// </remarks>
    public class EmployeeInfo
    {
        /// <summary>
        /// 取得或設定名稱。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// 取得或設定職級。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public int Level { get; set; }

        /// <summary>
        /// 取得或設定職稱。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public TitleEnum Title { get; set; }

        public List<string> TEL { get; set; }

        public List<XElement > ToDoList { get; set; }
    }
}
