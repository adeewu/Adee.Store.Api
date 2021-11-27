using System;
using System.Collections.Generic;

namespace Adee.Store
{
    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PageResultModel<T> : PageModel
    {
        /// <summary>
        /// 总数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPage
        {
            get
            {
                if (PageSize == 0) return 0;

                var page = TotalCount / PageSize;
                if (TotalCount % PageSize != 0) page += 1;

                return page;
            }
        }

        /// <summary>
        /// 当前页数据
        /// </summary>
        public IReadOnlyList<T> Items { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public Dictionary<string, string> Infos { get; set; }

        private PageResultModel() { }

        public PageResultModel(long totalCount, IReadOnlyList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}