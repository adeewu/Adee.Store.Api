using System;
using System.Text.RegularExpressions;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 业务订单号
    /// </summary>
    public class BusinessOrderId
    {
        /// <summary>
        /// 
        /// </summary>
        public BusinessOrderId()
        {
            Random = new Random(GetHashCode()).Next(10, 99);
            OrderTime = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessOrderId"></param>
        /// <returns></returns>
        public static BusinessOrderId Create(string businessOrderId)
        {
            CheckHelper.AreEqual(businessOrderId.Length, 22, message: "订单号长度必须为22");
            CheckHelper.IsTrue(Regex.IsMatch(businessOrderId, @"\d{22}"), "订单号必须为纯数字");

            return new BusinessOrderId
            {
                OrderTime = DateTime.ParseExact(businessOrderId.Substring(0, 12), "yyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture),
                SoftwareCode = businessOrderId.Substring(12, 8),
                Random = Convert.ToInt32(businessOrderId.Substring(20, 2)),
            };
        }

        /// <summary>
        /// 软件编号
        /// </summary>
        public string SoftwareCode { get; set; }

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
            return $"{SoftwareCode}{OrderTime:yyMMddHHmmss}{Random}";
        }
    }
}
