using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magikarp.Utility.TransitData
{
    /// <summary>
    /// 定義中介資料與資料物件轉換器操作介面。
    /// </summary>
    /// <typeparam name="TTransitData">中介資料型別。</typeparam>
    /// <remarks>
    /// Author: 黃竣祥
    /// Version: 20170921
    /// </remarks>
    public interface ITransitDataAdapter<TTransitData> where TTransitData : class
    {

        #region -- 方法 ( Public Method ) --

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
        TModel Loading<TModel>(TTransitData pi_objSource) where TModel : new();
        
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
        TTransitData Export<TModel>(TModel pi_objSource);

        #endregion

    }
}
