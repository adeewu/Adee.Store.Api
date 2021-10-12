using System;
using System.Collections.Generic;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 任务结果
    /// </summary>
    public class PayResponse
    {
        /// <summary>
        /// 任务状态
        /// </summary>
        public PayTaskStatus Status { get; set; }

        /// <summary>
        /// 原始响应报文
        /// </summary>
        public string OriginResponse { get; set; }

        /// <summary>
        /// 解密报文
        /// </summary>
        public string EncryptResponse { get; set; }

        /// <summary>
        /// 原始提交报文
        /// </summary>
        public string OriginRequest { get; set; }

        /// <summary>
        /// 提交报文
        /// </summary>
        public string SubmitRequest { get; set; }

        /// <summary>
        /// 任务消息
        /// </summary>
        public string ResponseMessage { get; set; }
    }

    public class PaySuccessResponse : PayResponse
    {
        /// <summary>
        /// 收款时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 收款金额，单位：分
        /// </summary>
        public int? Money { get; set; }

        /// <summary>
        /// 收单机构订单号
        /// </summary>
        public string PayOrganizationOrderId { get; set; }
    }

    /// <summary>
    /// 断定通知响应
    /// </summary>
    public class AssertNotifyResponse : PaySuccessResponse
    {
        /// <summary>
        /// 支付订单号
        /// </summary>
        public PayOrderId PayOrderId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public Guid? TenantId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PayRequest
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public PayTaskType PayTaskType { get; set; }

        /// <summary>
        /// 支付参数
        /// </summary>
        public string PayParameterValue { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PayOrderId { get; set; }
    }

    /// <summary>
    /// 退款
    /// </summary>
    public class RefundPayRequest : PayRequest
    {
        public RefundPayRequest()
        {
            PayTaskType = PayTaskType.Refund;
        }

        /// <summary>
        /// 支付金额，分
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// 退款订单号
        /// </summary>
        public string RefundOrderId { get; set; }

        /// <summary>
        /// 触发域名
        /// </summary>
        public string TargetDomain { get; set; }
    }

    /// <summary>
    /// B2C
    /// </summary>
    public class B2CPayRequest : PayRequest
    {
        public B2CPayRequest()
        {
            PayTaskType = PayTaskType.B2C;
        }

        /// <summary>
        /// 付款码
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 支付金额，分
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// 收款标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ip地址，优先取用户IP，再取商户IP，最后取服务器IP
        /// </summary>
        public string IPAddress { get; set; }
    }

    /// <summary>
    /// 断定通知
    /// </summary>
    public class AssertNotifyRequest : PayRequest
    {
        public AssertNotifyRequest()
        {
            PayTaskType = PayTaskType.AssertNotify;
            Headers = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// 通知码
        /// </summary>
        public string HashCode { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求正文
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 头部
        /// </summary>
        public Dictionary<string, string[]> Headers { get; set; }
    }
}
