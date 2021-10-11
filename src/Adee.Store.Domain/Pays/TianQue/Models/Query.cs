namespace Adee.Store.Domain.Pays.TianQue.Models
{
    public class QueryRequestModel
    {
        /// <summary>
        /// String	M	15	商户入驻返回的商户编号	399290754110000
        /// </summary>
        public string mno { get; set; }
        /// <summary>
        /// String	三选一	64	商户订单号	2017031601582703488262000972
        /// </summary>
        public string ordNo { get; set; }
        /// <summary>
        /// String	32	科技公司订单 号	ad6feaf120db4cb1a0002500003df487
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// String	64	支付宝或微信单号	2013112011001004330000001536
        /// </summary>
        public string transactionId { get; set; }
    }

    public class QueryResponse : Response
    {

        /// <summary>
        /// String	M	64	商户订单号	2017031601582703488262800072
        /// </summary>
        public string ordNo { get; set; }
        /// <summary>
        /// String	M	32	科技公司订单号	ad6feaf120db4cb1a0002500523df487
        /// </summary>
        public string uuid { get; set; }
        /// <summary>
        /// String	C	32	落单号，供退款和退款查询使用	836201015336679403
        /// </summary>
        public string sxfUuid { get; set; }
        /// <summary>
        /// String	M	14	交易支付完成时间	20180528130000
        /// </summary>
        public string payTime { get; set; }
        /// <summary>
        /// String	M	14	交易时间	20180529121209
        /// </summary>
        public string tranTime { get; set; }
        /// <summary>
        /// String	O	32	渠道商商户号	2001932193121
        /// </summary>
        public string channelId { get; set; }
        /// <summary>
        /// String	O	32	支付宝买家登录账号	135*2345
        /// </summary>
        public string buyerAccount { get; set; }
        /// <summary>
        /// String	M	9	交易金额	88.88
        /// </summary>
        public string oriTranAmt { get; set; }
        /// <summary>
        /// String	O	9	商家入账金额
        /// 说明：
        /// 包含手续费、预充值、平台补贴（优惠），不含免充值代金券（商家补贴）	99.09
        /// </summary>
        public string settleAmt { get; set; }
        /// <summary>
        /// String	O	9	消费者付款金额	88.88
        /// </summary>
        public string totalOffstAmt { get; set; }
        /// <summary>
        /// String	O	1	支付场景
        /// 1：刷卡
        /// 2：声波
        /// 3：刷脸
        /// 不上传默认为1	1
        /// </summary>
        public string scene { get; set; }
        /// <summary>
        /// String	M	2	是否做分账
        /// 分账交易使用；
        /// 00：做； 01：不做；
        /// 02:同步分账
        /// 不传默认为不做分账
        /// </summary>
        public string ledgerAccountFlag { get; set; }
        /// <summary>
        /// String	C	2	是否已做分账  00:是，01:否
        /// </summary>
        public string ledgerAccountStatus { get; set; }
        /// <summary>
        /// String	C	128	银联对账key
        /// </summary>
        public string reconKey { get; set; }
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
        /// String	O	9	付款银行，例如ICBC
        /// </summary>
        public string payBank { get; set; }
        /// <summary>
        /// String	M	256	订单标题	iphone8 plus
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// String	O	128	备用	正交易上送则原路返回
        /// </summary>
        public string extend { get; set; }
        /// <summary>
        /// String	C	64	买家用户号
        /// 支付宝渠道：买家支付宝用户号buyer_user_id
        /// 微信渠道：微信平台的sub_openid	2088101110005611
        /// </summary>
        public string buyerId { get; set; }
        /// <summary>
        /// String	M	16	交易状态：SUCCESS
        /// SUCCESS 交易成功，
        /// FAIL 交易失败，
        /// PAYING 支付中，
        /// NOTPAY 未支付，
        /// CLOSED 已关闭，
        /// CANCELED 已撤销
        /// </summary>
        public string tranSts { get; set; }
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
        /// String	O	200	姓名；实名支付时该参数必反
        /// </summary>
        public string buyerNameEnc { get; set; }
        /// <summary>
        /// String	O	64	证件类型；实名支付该参数必反 00
        /// </summary>
        public string buyerIdType { get; set; }
        /// <summary>
        /// 	String	O	100	证件号；实名支付时该参数必反	00
        /// </summary>
        public string buyerIdNoEnc { get; set; }
        /// <summary>
        /// 微信/支付宝流水号
        /// 支付宝渠道：支付宝交易流水
        /// 微信渠道：微信交易流水	2013112011001004330000121536
        /// </summary>
        public string transactionId { get; set; }
        /// <summary>
        /// String	O	15	交易手续费率；单位%	0.21
        /// </summary>
        public string recFeeRate { get; set; }
        /// <summary>
        /// String	O	15	交易手续费；单位元	0.12
        /// </summary>
        public string recFeeAmt { get; set; }
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
        /// <summary>
        /// String	O	15	银联二维码1000以上借记卡手续费封顶值	100.00
        /// </summary>
        public string cappingFee { get; set; }
        /// <summary>
        /// String	O	10	交易查询响应码	0000
        /// </summary>
        public string tradeCode { get; set; }
        /// <summary>
        /// 	String	O	1024	交易查询响应信息	成功
        /// </summary>
        public string tradeMsg { get; set; }
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
