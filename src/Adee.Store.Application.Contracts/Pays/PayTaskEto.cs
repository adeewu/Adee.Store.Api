using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.EventBus;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 通用支付任务内容
    /// </summary>
    [EventName("PayTaskEto")]
    public class PayTaskEto
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskType TaskType { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string MerchantOrderId { get; set; }

        /// <summary>
        /// 任务内容
        /// </summary>
        public string Content { get; set; }
    }
}
