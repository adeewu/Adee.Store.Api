namespace Adee.Store.Domain.Pays.TianQue.Models
{
    public class B2CRequestModel : Request
    {
        /// <summary>
        /// String	M	64	授权码
        /// 通过扫码枪/声波获取设备获取，支付宝/微信/银联收付码，输入条形码数字	287634000
        /// </summary>
        public string authCode { get; set; }
        /// <summary>
        /// String	O	1	支付场景，
        /// 1：刷卡
        /// 3：刷脸
        /// 不上传默认为1	3
        /// </summary>
        public string scene { get; set; }
        /// <summary>
        /// payWay	String	M	5	支付方式
        /// 00 主扫
        /// 01 被扫
        /// 02 公众号/服务窗
        /// 03 小程序
        /// </summary>
        public string payWay { get; set; }
    }

    public class B2CResponseModel : Response
    {
        /// <summary>
        /// String	O	32	子公众号 微信subAppId wx0c0bc0**026b9981
        /// </summary>
        public string subAppid { get; set; }
        /// <summary>
        /// String	O	32	子商户号	2000100211
        /// </summary>
        public string subMechId { get; set; }
        /// <summary>
        /// String	O	32	渠道商商户号	2001932193121
        /// </summary>
        public string channelId { get; set; }
        /// <summary>
        /// String	M	64	商户订单号	2017031601582703488262800072
        /// </summary>
        public string ordNo { get; set; }
        /// <summary>
        /// String	M	32	科技公司订单号	ad6feaf120db4cb1a0002500523df487
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// String	O	32	支付宝买家登录账号	135*2345
        /// </summary>
        public string buyerAccount { get; set; }
        /// <summary>
        /// String	O	1	支付场景
        /// 1：刷卡
        /// 2：声波
        /// 3：刷脸
        /// 不上传默认为1	1
        /// </summary>
        public string scene { get; set; }
        /// <summary>
        /// String	M	14	交易支付完成时间	20180528130000
        /// </summary>
        public string payTime { get; set; }
        /// <summary>
        /// String	C	64	买家用户号
        /// 支付宝渠道：买家支付宝用户号buyer_user_id
        /// 微信渠道：微信平台的sub_openid	2088101110005611
        /// </summary>
        public string buyerId { get; set; }
        /// <summary>
        /// String	M	20	支付渠道，对订单的描述
        /// 取值范围：
        /// WECHAT:微信
        /// ALIPAY:支付宝
        /// UNIONPAY:银联
        /// </summary>
        public string payType { get; set; }
        /// <summary>
        /// String	M	5	支付方式
        /// 00 主扫
        /// 01 被扫
        /// 02 公众号/服务窗
        /// 03 小程序
        /// </summary>
        public string payWay { get; set; }
        /// <summary>
        /// 微信/支付宝流水号
        /// 支付宝渠道：支付宝交易流水
        /// 微信渠道：微信交易流水	2013112011001004330000121536
        /// </summary>
        public string transactionId { get; set; }
        /// <summary>
        /// String	C	128	银联对账key
        /// </summary>
        public string reconKey { get; set; }
        /// <summary>
        /// String	O	2	借贷标识	借贷记标识
        /// 1-借,
        /// 2-贷，
        /// 3-其他
        /// </summary>
        public string drType { get; set; }
        /// <summary>
        /// String	O	15	交易手续费率；单位%	0.21
        /// </summary>
        public string recFeeRate { get; set; }
        /// <summary>
        /// String	O	15	交易手续费；单位元	0.12
        /// </summary>
        public string recFeeAmt { get; set; }
        /// <summary>
        /// String	O	9	消费者付款金额	88.88
        /// </summary>
        public string totalOffstAmt { get; set; }
        /// <summary>
        /// String	O	9	商家入账金额
        /// 说明：
        /// 包含手续费、预充值、平台补贴（优惠），不含免充值代金券（商家补贴）	99.09
        /// </summary>
        public string settleAmt { get; set; }
        /// <summary>
        /// String	O	9	代金券金额
        /// 积分支付金额，优惠金额或折扣券的金额	0.00
        /// </summary>
        public string pointAmount { get; set; }
        /// <summary>
        /// String	C	32	落单号，供退款和退款查询使用	836201015336679403
        /// </summary>
        public string sxfUuid { get; set; }
        /// <summary>
        /// String	O	4	00:线下标准
        /// 01:线上标准
        /// 02:蓝海
        /// 03:绿洲
        /// 04:线下缴费
        /// 05:线下保险
        /// 06:线下非赢利
        /// 07:高校食堂
        /// 08：私立院校
        /// 09：其他
        /// 10：教培机构	00
        /// </summary>
        public string activityNo { get; set; }
        /// <summary>
        /// String	O	128	备用	正交易上送则原路返回
        /// </summary>
        public string extend { get; set; }
        /// <summary>
        /// 优惠信息（jsonArray格式字符串）
        /// </summary>
        public string promotionDetail { get; set; }
        /// <summary>
        /// String	O	2	是否收支两条线标识	“00”,”是否收支两条线标识: 是”
        /// “01”,”是否收支两条线标识: 否”
        /// </summary>
        public string szltFlag { get; set; }
        /// <summary>
        /// String	O	15	后收手续费	0.00
        /// </summary>
        public string szltRecfeeAmt { get; set; }
        //String	O	8	清算日期	20201203
        public string clearDt { get; set; }
        /// <summary>
        /// String	O	14	交易完成时间	20201203100000
        /// </summary>
        public string finishTime { get; set; }
        /// <summary>
        /// String	O	2	切批时间	01
        /// </summary>
        public string settlementBatchNo { get; set; }
        /// <summary>
        /// String	O	32	终端号	银联仅为8位字符：支付宝微信不超过32位字符
        /// </summary>
        public string ylTrmNo { get; set; }
    }
}
