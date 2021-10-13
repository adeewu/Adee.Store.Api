using Adee.Store.Domain.Pays.TianQue.Models;
using Adee.Store.Pays;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Domain.Pays.TianQue
{
    [PayProvider(PayOrganizationType.TianQue)]
    public class TianQuePay : DefaultPayProvider, ITransientDependency
    {
        private readonly TianQueClient _client;

        public TianQuePay(TianQueClient client)
        {
            _client = client;
        }

        public override async Task<SuccessResponse> Refund(RefundRequest request)
        {
            var refundRequest = _client.GetRequest(request.PayParameterValue, new RefundRequestModel
            {
                amt = Math.Round(request.Money / 100m, 2).ToString("#0.00"),
                ordNo = request.RefundOrderId,
                origOrderNo = request.PayOrderId.ToString(),
            });

            var response = await _client.Refund(refundRequest);
            CheckHelper.IsNotNull(response, name: nameof(response));
            CheckHelper.IsNotNull(response.Response, name: nameof(response.Response));

            var result = new SuccessResponse
            {
                Status = PayTaskStatus.Faild,
                EncryptResponse = response.EncryptResponse,
                OriginResponse = response.OriginResponse,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                ResponseMessage = response.Response.GetPropValue(p => p.respData).GetPropValue(p => p.bizMsg) ?? response.Response.GetPropValue(p => p.msg),
            };

            if (response.Response.code != "0000" || response.Response.respData == null)
            {
                result.ResponseMessage = $"(返回码：{response.Response.code}){response.Response.msg}";
                return result;
            }

            if (response.Response.respData.bizCode == "0000")
            {
                result.Status = PayTaskStatus.Success;
                result.PayTime = response.Response.respData.finishTime.ToDateTime("YYYYMMddHHmmss");
                result.Money = (int)(response.Response.respData.amt.To<decimal>() * 100);
                result.PayOrganizationOrderId = response.Response.respData.sxfUuid;
            }

            if (response.Response.respData.bizCode == "2002")
            {
                result.Status = PayTaskStatus.Executing;
            }

            return result;
        }

        public override async Task<SuccessResponse> B2C(B2CRequest request)
        {
            var b2cRequest = _client.GetRequest(request.PayParameterValue, new B2CRequestModel
            {
                amt = Math.Round(request.Money / 100m, 2).ToString("#0.00"),
                authCode = request.AuthCode,
                subject = string.IsNullOrWhiteSpace(request.Title) ? "订单支付" : request.Title,
                ordNo = request.PayOrderId.ToString(),
                tradeSource = "01",
                trmIp = request.IPAddress,
                notifyUrl = request.NotifyUrl,
            });

            var response = await _client.B2CPay(b2cRequest);
            CheckHelper.IsNotNull(response, name: nameof(response));
            CheckHelper.IsNotNull(response.Response, name: nameof(response.Response));

            var result = new SuccessResponse
            {
                Status = PayTaskStatus.Faild,
                EncryptResponse = response.EncryptResponse,
                OriginResponse = response.OriginResponse,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                ResponseMessage = response.Response.GetPropValue(p => p.respData).GetPropValue(p => p.bizMsg) ?? response.Response.GetPropValue(p => p.msg),
            };

            if (response.Response.code != "0000" || response.Response.respData == null)
            {
                result.ResponseMessage = $"(返回码：{response.Response.code}){response.Response.msg}";
                return result;
            }

            if (response.Response.respData.bizCode == "0000")
            {
                result.Status = PayTaskStatus.Success;
                result.PayTime = response.Response.respData.payTime.ToDateTime("YYYYMMddHHmmss");
                result.Money = request.Money;
                result.PayOrganizationOrderId = response.Response.respData.sxfUuid;
            }

            if (response.Response.respData.bizMsg == "用户支付中，请稍后进行查询")
            {
                result.Status = PayTaskStatus.Executing;
            }

            return result;
        }

        public override async Task<JsApiResponse> JSApi(JSApiRequest request)
        {
            var jsApiRequest = _client.GetRequest(request.PayParameterValue, new JSApiRequestModel
            {
                ordNo = request.PayOrderId,
                amt = Math.Round(request.Money / 100m, 2).ToString("#0.00"),
                payType = GetPayChannel(request.PaymentType),
                payWay = request.TradeType == JSApiTradeType.MINIPRO ? "03" : "02",
                timeExpire = request.PayExpire.ToString(),
                subject = request.Title,
                tradeSource = "01",
                trmIp = request.IPAddress,
                notifyUrl = request.NotifyUrl,
                userId = request.UserId,
                subAppid = request.SubAppId,
                customerIp = request.IPAddress,
            });

            var response = await _client.JSApiPay(jsApiRequest);

            var result = new JsApiResponse
            {
                Status = PayTaskStatus.Faild,
                EncryptResponse = response.EncryptResponse,
                OriginResponse = response.OriginResponse,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                ResponseMessage = response.Response.GetPropValue(p => p.respData).GetPropValue(p => p.bizMsg) ?? response.Response.GetPropValue(p => p.msg),
            };

            if (response.Response.code != "0000" || response.Response.respData == null)
            {
                result.ResponseMessage = $"(返回码：{response.Response.code}){response.Response.msg}";
                return result;
            }

            if (response.Response.respData.bizCode == "0000")
            {
                result.Status = PayTaskStatus.Success;
                result.PayOrganizationOrderId = response.Response.respData.sxfUuid;

                if (request.PaymentType == PaymentType.WechatPay)
                {
                    result.Parameter = new WechatJSApiResult
                    {
                        appId = response.Response.respData.payAppId,
                        nonceStr = response.Response.respData.paynonceStr,
                        package = response.Response.respData.payPackage,
                        paySign = response.Response.respData.paySign,
                        signType = response.Response.respData.paySignType,
                        timeStamp = response.Response.respData.payTimeStamp,
                    }.ToJsonString();
                }
                if (request.PaymentType == PaymentType.Alipay)
                {
                    result.Parameter = response.Response.respData.source;
                }
                if (request.PaymentType == PaymentType.UnionPay)
                {
                    result.Parameter = response.Response.respData.redirectUrl;
                }

                if (result.Parameter.IsNullOrWhiteSpace())
                {
                    result.Status = PayTaskStatus.Faild;
                    result.ResponseMessage = $"未能正确解析到支付参数，付款方式：{request.PaymentType}";
                }
            }

            return result;
        }

        public override async Task<SuccessResponse> Query(PayTaskRequest request)
        {
            var queryRequest = _client.GetRequest(request.PayParameterValue, new QueryRequestModel
            {
                ordNo = request.PayOrderId
            });

            var response = await _client.Query(queryRequest);

            var result = new SuccessResponse
            {
                Status = PayTaskStatus.Faild,
                EncryptResponse = response.EncryptResponse,
                OriginResponse = response.OriginResponse,
                OriginRequest = response.OriginRequest,
                SubmitRequest = response.SubmitRequest,
                ResponseMessage = response.Response.GetPropValue(p => p.respData).GetPropValue(p => p.bizMsg) ?? response.Response.GetPropValue(p => p.msg),
            };

            if (response.Response.code != "0000" || response.Response.respData == null)
            {
                result.ResponseMessage = $"(返回码：{response.Response.code}){response.Response.msg}";
                return result;
            }

            if (response.Response.respData.tranSts == "FAIL"
                || response.Response.respData.tranSts == "CANCELED"
                || response.Response.respData.tranSts == "NOTPAY"
                || response.Response.respData.tranSts == "CLOSED")
            {
                result.ResponseMessage = $"【{response.Response.respData.tranSts}】{result.ResponseMessage}";
                return result;
            }

            if (response.Response.respData.tranSts == "SUCCESS")
            {
                result.Status = PayTaskStatus.Success;
                result.PayTime = response.Response.respData.payTime.ToDateTime("YYYYMMddHHmmss");
                result.PayOrganizationOrderId = response.Response.respData.sxfUuid;
                result.Money = Convert.ToInt32(Convert.ToDecimal(response.Response.respData.oriTranAmt) * 100);

                return result;
            }

            if (response.Response.respData.tranSts == "PAYING")
            {
                result.Status = PayTaskStatus.Executing;

                return result;
            }

            result.ResponseMessage = $"未实现的交易状态：{response.Response.respData.tranSts}";
            return result;
        }

        private string GetPayChannel(PaymentType paymethodType)
        {
            var payChannel = string.Empty;

            if (paymethodType == PaymentType.Alipay) payChannel = "ALIPAY";
            if (paymethodType == PaymentType.WechatPay) payChannel = "WECHAT";
            if (paymethodType == PaymentType.UnionPay) payChannel = "UNIONPAY";

            CheckHelper.IsNotNull(payChannel, $"不支持的付款方式：{paymethodType}");

            return payChannel;
        }

    }
}
