namespace Adee.Store.Domain.Pays.TianQue.Models
{
    public class RefundRequestModel
    {

        /// <summary>
        /// String	M	15	商户入驻返回的商户编号
        /// </summary>
        public string mno { get; set; }
        /// <summary>
        /// String	M	64	商户订单号,64个字符以内、        只能包含字母、数字、下划线；需保证在商户端不重复
        /// </summary>
        public string ordNo { get; set; }
        /// <summary>
        /// String	至少传其一	64	原交易商户订单号,64个字符以内、只能包含字母、数字、下划线；需保证在商户端不重复	2017031601582703488262843971
        /// </summary>
        public string origOrderNo { get; set; }
        /// <summary>
        /// String	32	原交易科技公司订单号	ad6feaf120db4cb1a0002500523df487
        /// </summary>
        public string origUuid { get; set; }
        /// <summary>
        /// String	32	正交易落单号	836201015336679403
        /// </summary>
        public string origSxfUuid { get; set; }
        /// <summary>
        /// String	M	9	退货金额，单元	88.88
        /// </summary>
        public string amt { get; set; }
        /// <summary>
        /// String	O	256	支付结果通知地址	https://www.nify.com/nofy.htm
        /// </summary>
        public string notifyUrl { get; set; }
        /// <summary>
        /// String	O	80	退货原因	退货
        /// </summary>
        public string refundReason { get; set; }
        /// <summary>
        /// String	O	128	备用
        /// </summary>
        public string extend { get; set; }
        /// <summary>
        /// String	O	2	订单优惠标识00：是，01：否；	00
        /// </summary>
        public string goodsTag { get; set; }
        /// <summary>
        /// String	O	-	优惠详情信息(json格式，里面内容见下面说明)（支持支付宝、银联）
        /// </summary>
        public string couponDetail { get; set; }
    }

    public class RefundResponseModel : Response
    {
        /// <summary>
        /// String	C	32	落单号，供退款和退款查询使用	836201015336679403
        /// </summary>
        public string sxfUuid { get; set; }
        /// <summary>
        /// String	M	64	商户原订单号	2017031601582703488262000972
        /// </summary>
        public string origOrderNo { get; set; }
        /// <summary>
        /// String	M	32	科技公司原订单号	ad6feaf120db4cb1a00025005000f487
        /// </summary>
        public string origUuid { get; set; }
        /// <summary>
        /// String	M	32	科技公司订单号	ad6feaf120db4cb1a00025005000f481
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// String	O	64	三方订单号
        /// </summary>
        public string transactionId { get; set; }
        /// <summary>
        /// String	M	9	申请退款金额，反交易下单的退款金额
        /// </summary>
        public string amt { get; set; }
        /// <summary>
        /// String	O	-	优惠券退款信息（jsonArray格式字符串）具体参数见下面说明	0.00
        /// </summary>
        public string refundDetail { get; set; }
        /// <summary>
        /// String	C	64	买家用户号微信是subopenid；支付宝是userId	2088101110005611
        /// </summary>
        public string buyerId { get; set; }
        /// <summary>
        /// String	O	9	消费者到账金额	90.09
        /// </summary>
        public string refBuyerAmt { get; set; }
        /// <summary>
        /// String	O	80	退货原因	退货
        /// </summary>
        public string refundReason { get; set; }
        /// <summary>
        /// String	O	128	备用
        /// </summary>
        public string extend { get; set; }
        /// <summary>
        /// String	M	20	支付渠道，对订单的描述,
        /// 取值范围 ：
        /// WECHA：微信
        /// ALIPAY：支付宝
        /// UNIONPAY:银联
        /// </summary>
        public string payType { get; set; }
        /// <summary>
        /// String	M	5	支付方式
        /// 00 主扫,
        /// 01 被扫,
        /// 02 公众号/服务窗,
        /// 03 小程序
        /// </summary>
        public string payWay { get; set; }
        /// <summary>
        /// String	O	2	是否收支两条线标识	“00”,”是否收支两条线标识: 是”
        /// “01”,”是否收支两条线标识: 否”
        /// </summary>
        public string szltFlag { get; set; }
        /// <summary>
        /// String	O	15	后收手续费	0.00
        /// </summary>
        public string szltRecfeeAmt { get; set; }
        /// <summary>
        /// String	O	8	清算日期	20201203
        /// </summary>
        public string sclearDt { get; set; }
        /// <summary>
        /// String	O	14	交易完成时间	20201203100000
        /// </summary>
        public string finishTime { get; set; }
        /// <summary>
        /// String	O	2	切批时间	01
        /// </summary>
        public string settlementBatchNo { get; set; }
        /// <summary>
        /// String	O	8	终端号	银联仅为8位字符：支付宝微信不超过32位字符
        /// </summary>
        public string ylTrmNo { get; set; }
    }
}
