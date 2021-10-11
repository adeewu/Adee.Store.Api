namespace Adee.Store.Pays
{
    /// <summary>
    /// 扫码支付
    /// </summary>
    public class B2C
    {
        /// <summary>
        /// 收款码
        /// </summary>
        public string AuthCode { get; set; }

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
    }
}
