using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Magikarp.Utility.TransitData
{
    /// <summary>
    /// 定義中介資料與資料物件的個別屬性轉換器操作介面。
    /// </summary>
    /// <typeparam name="TTransitData">中介資料型別。</typeparam>
    /// <remarks>
    /// Author: 黃竣祥
    /// Version: 20170921
    /// </remarks>
    public interface IDataConverter<TTransitData> 
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
        void SetValue(object pi_objContainer, System.Reflection.PropertyInfo pi_objProperty, XElement pi_objValue);

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
        TTransitData Export(object pi_objContainer, System.Reflection.PropertyInfo pi_objProperty);

        #endregion

        #region -- 屬性 ( Properties ) --

        /// <summary>
        /// 取得或設定後繼處理實體。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/21
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        IDataConverter<TTransitData> Successor { get; set; }

        #endregion

    }
}
