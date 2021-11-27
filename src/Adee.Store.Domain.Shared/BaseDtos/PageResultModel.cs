using System;
using System.Collections.Generic;

namespace Adee.Store
{
    /// <summary>
    /// ��ҳ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PageResultModel<T> : PageModel
    {
        /// <summary>
        /// ����
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// ��ҳ��
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
        /// ��ǰҳ����
        /// </summary>
        public IReadOnlyList<T> Items { get; set; }

        /// <summary>
        /// �Զ�������
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