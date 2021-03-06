using Adee.Store.Domain.Pays.TianQue.Models;
using Adee.Store.Domain.Shared.Utils.Helpers;
using Adee.Store.Pays;
using Adee.Store.Pays.Utils.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Domain.Pays.TianQue
{
    public class TianQueClient : ITransientDependency
    {
        private readonly ICommonClient _client;
        private readonly SignHelper _signHelper;

        public PaymentConfigModel PaymentConfig { get; set; }

        public TianQueClient(
            ICommonClient client,
            SignHelper signHelper,
            IOptions<TianQueOptions> options)
        {
            _signHelper = signHelper;

            _client = client;
            _client.SetBaseAddress(options.Value.IsTest ? "https://openapi-test.tianquetech.com" : "https://openapi.tianquetech.com");
        }

        public void SetPayparameterValue(string payParameterValue)
        {
            PaymentConfig = payParameterValue.AsObject<PaymentConfigModel>();
        }

        /// <summary>
        /// 获取基础的请求参数
        /// </summary>
        /// <returns></returns>
        public RequestBase<T> GetRequest<T>(T t)
        {
            var request = new RequestBase<T>
            {
                orgId = PaymentConfig.orgId,
                reqId = DateTimeOffset.Now.ToUnixTimeMilliseconds() + new Random(GetHashCode()).Next(1000, 9999).ToString(),
                version = "1.0",
                timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"),
                signType = "RSA",
                reqData = t
            };
            request.reqData.SetPropValue(nameof(PaymentConfig.mno), PaymentConfig.mno);

            return request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DetailResponse<ResponseBase<C2BResponseModel>>> C2BPay(RequestBase<C2BRequestModel> request)
        {
            var dic = Sign(request);

            var result = await PostAsync<C2BResponseModel>("/order/activeScan", body: dic);
            result.OriginRequest = request.ToJsonString();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DetailResponse<ResponseBase<B2CResponseModel>>> B2CPay(RequestBase<B2CRequestModel> request)
        {
            var dic = Sign(request);

            var result = await PostAsync<B2CResponseModel>("/order/reverseScan", body: dic);
            result.OriginRequest = request.ToJsonString();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DetailResponse<ResponseBase<JSApiResponseModel>>> JSApiPay(RequestBase<JSApiRequestModel> request)
        {
            var dic = Sign(request);

            var result = await PostAsync<JSApiResponseModel>("/order/jsapiScan", body: dic);
            result.OriginRequest = request.ToJsonString();

            return result;
        }

        /// <summary>
        /// 异步post方式
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url">url</param>
        /// <param name="query">url参数</param>
        /// <param name="body">提交body</param>
        /// <returns></returns>
        public async Task<DetailResponse<ResponseBase<TResult>>> PostAsync<TResult>(string url, object query = null, object body = null)
        {
            var result = new DetailResponse<ResponseBase<TResult>>();

            var requestMessage = await _client.GetHttpRequestMessage(HttpMethod.Post, url, query, body);
            result.SubmitRequest = requestMessage.ToString();

            var responseMessage = await _client.GetHttpResponseMessage(requestMessage);

            var response = await _client.ReadStringAsync(responseMessage);
            result.OriginResponse = responseMessage.ToString();

            VerifySign(response);

            result.Response = response.AsObject<ResponseBase<TResult>>();

            return result;
        }

        /// <summary>
        /// 支付查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DetailResponse<ResponseBase<QueryResponse>>> Query(RequestBase<QueryRequestModel> request)
        {
            var dic = Sign(request);

            return await PostAsync<QueryResponse>("/query/tradeQuery", body: dic);
        }

        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DetailResponse<ResponseBase<RefundResponseModel>>> Refund(RequestBase<RefundRequestModel> request)
        {
            var dic = Sign(request);

            var result = await PostAsync<RefundResponseModel>("/order/refund", body: dic);
            result.OriginRequest = request.ToJsonString();

            return result;
        }

        /// <summary>
        /// 结果验签
        /// </summary>
        /// <param name="responseResult"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public void VerifySign(string responseResult)
        {
            var dic = responseResult.AsObject<Dictionary<string, object>>();

            var sign = dic.Where(p => p.Key == "sign").Select(p => p.Value.ToString()).FirstOrDefault();
            CheckHelper.IsNotNull(sign, "未包含sign信息");

            var signDic = dic.Where(p => p.Key != "respData").Where(p => p.Key != "sign").ToDictionary(p => p.Key, p => p.Value.ToString());

            var respData = dic.Where(p => p.Key == "respData").Select(p => p.Value).FirstOrDefault();
            if (respData != null)
            {
                signDic.Add("respData", respData.ToJsonString());
            }

            var encryptString = _signHelper.Sign(signDic, string.Empty, "sign", separator: "&", containKey: true, justCombine: true, isLowerKey: false);

            var verifyResult = _signHelper.Verify(PaymentConfig.PublicKey, encryptString, sign);
            CheckHelper.IsTrue(verifyResult, $"支付平台验签失败，验签内容：{responseResult}");
        }

        /// <summary>
        /// 加签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private Dictionary<string, string> Sign<T>(RequestBase<T> request)
        {
            var dic = request.AsObject<Dictionary<string, object>>();

            CheckHelper.IsNotNull(request.reqData, $"请求参数：{nameof(request.reqData)}不能为空");
            var reqData = request
                .reqData
                .AsObject<Dictionary<string, string>>()
                .Where(p => string.IsNullOrWhiteSpace(p.Value) == false)
                .ToDictionary(p => p.Key, p => p.Value)
                .ToJsonString();
            dic[nameof(request.reqData)] = reqData;

            var signDic = dic.Where(p => p.Key != nameof(request.sign)).ToDictionary(p => p.Key, p => p.Value.ToString());

            var encryptString = _signHelper.Sign(signDic, string.Empty, "sign", sortKey: true, separator: "&", containKey: true, justCombine: true, isLowerKey: false);
            var sign = _signHelper.SignRSA(PaymentConfig.PrivateKey, encryptString);

            signDic.Add(nameof(request.sign), sign);
            return signDic;
        }
    }
}
