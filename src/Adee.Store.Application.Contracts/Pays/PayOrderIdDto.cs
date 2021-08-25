using System;
using System.Text.RegularExpressions;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付订单Id
    /// </summary>
    public class PayOrderIdDto
    {
        /// <summary>
        /// 
        /// </summary>
        public PayOrderIdDto()
        {
            Random = new Random(GetHashCode()).Next(10, 99);
            OrderTime = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payOrderId"></param>
        public PayOrderIdDto(string payOrderId) : this()
        {
            AssertHelper.AreEqual(payOrderId.Length, 30, "支付订单号长度必须为30");
            AssertHelper.IsTrue(Regex.IsMatch(payOrderId, @"\d{30}"), "支付订单号必须为纯数字");

            OrderTime = DateTime.ParseExact(payOrderId.Substring(0, 12), "yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            TenantId = payOrderId.Substring(12, 8);
            OrganizationType = (PayOrganizationType)Convert.ToInt32(payOrderId.Substring(20, 2));
            PaymentType = (PaymentType)Convert.ToInt32(payOrderId.Substring(22, 2));
            PaymethodType = (PaymethodType)Convert.ToInt32(payOrderId.Substring(24, 2));
            BusinessType = (BusinessType)Convert.ToInt32(payOrderId.Substring(26, 2));
            Random = Convert.ToInt32(payOrderId.Substring(28, 2));
        }

        /// <summary>
        /// 租户Id
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// 收单机构
        /// </summary>
        public PayOrganizationType OrganizationType { get; set; }

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
            var organizationTypeString = "{0:d2}".FormatString((int)OrganizationType);
            var paymentTypeString = "{0:d2}".FormatString((int)PaymentType);
            var paythodTypeString = "{0:d2}".FormatString((int)PaymethodType);
            var businessTypeString = "{0:d2}".FormatString((int)BusinessType);

            return $"{OrderTime:yyMMddHHmmss}{TenantId}{organizationTypeString}{paymentTypeString}{paythodTypeString}{businessTypeString}{Random}";
        }
    }
}
