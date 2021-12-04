using System;

namespace Adee.Store.ElasticSearchs
{
    /// <summary>
    /// 
    /// </summary>
    public class ElasticSearchLogInput : PageModel
    {
        /// <summary>
        /// ��������
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}