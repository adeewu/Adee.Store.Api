using System;
using System.Text.RegularExpressions;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付订单Id
    /// </summary>
    public class PayOrderId
    {
        /// <summary>
        /// 
        /// </summary>
        public PayOrderId()
        {
            Random = new Random(GetHashCode()).Next(10, 99);
            OrderTime = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payOrderId"></param>
        /// <returns></returns>
        public static PayOrderId Create(string payOrderId)
        {
            CheckHelper.AreEqual(payOrderId.Length, 32, message: "支付订单号长度必须为30");
            CheckHelper.IsTrue(Regex.IsMatch(payOrderId, @"\d{32}"), "支付订单号必须为纯数字");

            return new PayOrderId
            {
                OrderTime = DateTime.ParseExact(payOrderId.Substring(0, 12), "yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture),
                SoftwareCode = payOrderId.Substring(12, 10),
                PayOrganizationType = (PayOrganizationType)Convert.ToInt32(payOrderId.Substring(22, 2)),
                PaymentType = (PaymentType)Convert.ToInt32(payOrderId.Substring(24, 2)),
                PaymethodType = (PaymethodType)Convert.ToInt32(payOrderId.Substring(26, 2)),
                BusinessType = (BusinessType)Convert.ToInt32(payOrderId.Substring(28, 2)),
                Random = Convert.ToInt32(payOrderId.Substring(30, 2)),
            };
        }

        /// <summary>
        /// 软件编号
        /// </summary>
        public string SoftwareCode { get; set; }

        /// <summary>
        /// 收单机构
        /// </summary>
        public PayOrganizationType PayOrganizationType { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymethodType PaymethodType { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType BusinessType { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime OrderTime { get; private set; }

        /// <summary>
        /// 随机支付
        /// </summary>
        public int Random { get; private set; }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var organizationTypeString = "{0:d2}".FormatString((int)PayOrganizationType);
            var paymentTypeString = "{0:d2}".FormatString((int)PaymentType);
            var paythodTypeString = "{0:d2}".FormatString((int)PaymethodType);
            var businessTypeString = "{0:d2}".FormatString((int)BusinessType);

            return $"{OrderTime:yyMMddHHmmss}{SoftwareCode}{organizationTypeString}{paymentTypeString}{paythodTypeString}{businessTypeString}{Random}";
        }
    }
}
