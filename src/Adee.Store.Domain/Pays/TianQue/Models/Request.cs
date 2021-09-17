using System;
using System.Collections.Generic;

namespace Adee.Store.Domain.Pays.TianQue.Models
{
    /// <summary>
    /// 请求基类
    /// </summary>
    public class Request
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
        /// String	O	32	子商户号        说明：渠道子商户号”childNo”: “225505167”
        /// </summary>
        public string subMechId { get; set; }
        /// <summary>
        /// String	O	32	微信subAppId
        /// </summary>
        public string subAppid { get; set; }
        /// <summary>
        /// String	M	9	订单总金额，单位为元，精确到小数点后两位，取值范围[0.01, 100000000]
        /// </summary>
        public string amt { get; set; }
        /// <summary>
        /// String	O	9	参与优惠金额，单位元，精确到小数点后两位[0.01,100000000]；支付宝专用
        /// </summary>
        public string discountAmt { get; set; }
        /// <summary>
        /// String	O	9	不参与优惠金额，单位元，精确到小数点后两位[0.01,100000000]；支付宝专用
        /// </summary>
        public string unDiscountAmt { get; set; }
        /// <summary>
        /// String	M	20	支付渠道，对订单的描述，
        /// 取值范围：
        /// WECHAT:微信,
        /// ALIPAY:支付宝,
        /// UNIONPAY: 银联
        /// 企业资质均支持；
        /// 银联：自然人报备成功的支持使用
        /// </summary>
        public string payType { get; set; }

        /// <summary>
        /// String	M	256	订单标题
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// String	M	9	交易来源；
        /// 01：服务商
        /// 02：收银台
        /// 03：硬件
        /// </summary>
        public string tradeSource { get; set; }
        /// <summary>
        /// String	M	16	商家ip地址
        /// </summary>
        public string trmIp { get; set; }
        /// <summary>
        /// String	O	2	花呗分期数	仅可上送6或12
        /// </summary>
        public string hbFqNum { get; set; }
        /// <summary>
        /// String	O	2	限制卡类型:
        /// 00-全部支持
        /// 01-限定不能使用信用卡支付
        /// 默认值 00全部支持
        /// </summary>
        public string limitPay { get; set; }
        /// <summary>
        /// String	0	32	微信优惠参数
        /// </summary>
        public string wxGoodsTag { get; set; }
        /// <summary>
        /// 	String	O	2	订单优惠标识  00：是，01： 否；
        /// </summary>
        public string goodsTag { get; set; }
        /// <summary>
        /// String	O	-	优惠详情信息(json格式，里面内容见下面说明)
        /// </summary>
        public string couponDetail { get; set; }
        /// <summary>
        /// String	O	5	电子发票功能
        /// 微信开具电子发票使用；目前仅支持微信；
        /// 交易渠道为支付宝、银联时上送该参数则返回错误
        /// 00:是，01:否 ；
        /// </summary>
        public string needReceipt { get; set; }
        /// <summary>
        /// String	O	2	是否做异步分账
        /// 00：做； 01：不做；
        /// 不传默认为不做异步分账
        /// </summary>
        public string ledgerAccountFlag { get; set; }
        /// <summary>
        /// String	O	2	分账有效时间 单位为天；
        /// 是否做异步分账选择00时该字段必传；
        /// 最大支持上送：30 ；
        /// 注：从发起交易日期记为1
        /// </summary>
        public string ledgerAccountEffectTime { get; set; }
        /// <summary>
        /// JsonArray	O	-	同步分账规则	“fusruleId “:[{“mno”:”399123456789”,”a
        /// llotValue”:”10”},{“mno”: “399000000002”,”allotValue”:”20”}]
        /// </summary>
        public List<FusruleIdModel> fusruleId { get; set; }
        /// <summary>
        /// String	0	256	支付结果通知地址
        /// 注：不上送则交易成功后
        /// 无异步通知
        /// </summary>
        public string notifyUrl { get; set; }
        /// <summary>
        /// String	O	32	终端号	银联仅为8位字符：支付宝微信不超过32位字符
        /// </summary>
        public string ylTrmNo { get; set; }
        /// <summary>
        /// String	O	2	是否实名支付：
        /// 00：是，01：否
        /// 不传默认为01;00时；
        /// 买家姓名、证件类型、证件号为必填
        /// </summary>
        public string identityFlag { get; set; }
        /// <summary>
        /// String	O	32	证件类型: 大陆：IDCARD；
        /// 目前仅支持大陆证件号
        /// </summary>
        public string buyerIdType { get; set; }
        /// <summary>
        /// String	O	18	证件号:证件号只允许等于18位	410523198701054018
        /// </summary>
        public string buyerIdNo { get; set; }
        /// <summary>
        /// String	O	32	买家姓名
        /// </summary>
        public string buyerName { get; set; }
        /// <summary>
        /// String	O	12	手机号
        /// </summary>
        public string mobileNum { get; set; }
        /// <summary>
        /// String	0	128	备用
        /// </summary>
        public string extend { get; set; }
    }

    public class RequestBase<T>
    {
        /// <summary>
        /// 合作机构 id	是	随行付分配为准	随行付给合作机构分配的唯一id
        /// </summary>
        public string orgId { get; set; }
        /// <summary>
        /// 请求id	是	32	每次请求必须是唯一值，标明此报文的唯一id
        /// </summary>
        public string reqId { get; set; }
        /// <summary>
        /// 版本	是	3	格式：1.0
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 请求时间	是	14	yyyyMMddHHmmss格式
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 业务数据	是	-	对应业务信息信息
        /// </summary>
        public T reqData { get; set; }
        /// <summary>
        /// 签名结果	是	-	业务数据签名结果
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 签名类型	是	10	默认只支持RSA
        /// </summary>
        public string signType { get; set; }
    }

    /// <summary>
    /// 同步分账规则
    /// </summary>
    public class FusruleIdModel
    {
        /// <summary>
        /// 分账收款商编号
        /// </summary>
        public string mno { get; set; }
        /// <summary>
        /// 分账具体金额
        /// </summary>
        public decimal allotValue { get; set; }
    }

    /// <summary>
    /// 响应基类
    /// </summary>
    public class Response
    {
        /// <summary>
        /// String	M	10	业务返回码	0000
        /// </summary>
        public string bizCode { get; set; }
        /// <summary>
        /// String	M	1024	业务返回信息	成功 当返回码0000以下信息返回
        /// </summary>
        public string bizMsg { get; set; }
    }

    /// <summary>
    /// 响应实体
    /// </summary>
    public class ResponseBase<T>
    {
        /// <summary>
        /// 返回码	是	10	参见响应码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回信息	是	20	参见响应信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 请求id	否	32	请求时的订单号
        /// </summary>
        public string reqId { get; set; }
        /// <summary>
        /// 返回业务数据	否	-	code=0000才会返回业务信息信息
        /// </summary>
        public T respData { get; set; }
        /// <summary>
        /// 签名结果	否	-	code=0000才会加签
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 签名类型	是	10
        /// </summary>
        public string signType { get; set; }
        /// <summary>
        /// 合作机构id	是	随行付分配为准	随行付给合作机构分配的唯一id
        /// </summary>
        public string orgId { get; set; }
    }

}
