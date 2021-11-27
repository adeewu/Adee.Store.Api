using System;

namespace Adee.Store.ElasticSearchs
{
    /// <summary>
    /// 
    /// </summary>
    public class ElasticSearchLogInput : PageModel
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}