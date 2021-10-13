using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 
    /// </summary>
    public class BasePayTask
    {
        /// <summary>
        /// ip地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType BusinessType { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 业务订单号
        /// </summary>
        public string BusinessOrderId { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// 收款标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 收款备注
        /// </summary>
        public string PayRemark { get; set; }
    }
}
