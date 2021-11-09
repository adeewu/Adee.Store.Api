using Adee.Store.Attributes;
using Adee.Store.CallbackRequests;
using Adee.Store.Domain.Shared.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TenantManagement;

namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付通知服务
    /// </summary>
    [ApiGroup(ApiGroupType.Pay)]
    public class NotifyAppService : StoreWithRequestAppService, ITransientDependency
    {
        private readonly SignHelper _signHelper;
        private readonly IRepository<PayNotify> _payNotifyRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<CallbackRequest> _callbackRequestRepository;
        private readonly PayManager _payManager;

        public NotifyAppService(
            SignHelper signHelper,
            IRepository<PayNotify> payNotifyRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<CallbackRequest> callbackRequestRepository,
            PayManager payManager
            )
        {
            _signHelper = signHelper;
            _payNotifyRepository = payNotifyRepository;
            _tenantRepository = tenantRepository;
            _callbackRequestRepository = callbackRequestRepository;
            _payManager = payManager;
        }

        /// <summary>
        /// Get方式通知
        /// </summary>
        /// <param name="__tenant"></param>
        /// <returns></returns>
        public async Task<object> Get(string __tenant)
        {
            CheckHelper.IsNotNull(__tenant, $"{nameof(__tenant)}不能为空");

            var result = await Save();

            return result;
        }

        /// <summary>
        /// Post方式通知
        /// </summary>
        /// <param name="__tenant"></param>
        /// <returns></returns>
        public async Task<object> Post(string __tenant)
        {
            CheckHelper.IsNotNull(__tenant, $"{nameof(__tenant)}不能为空");

            var result = await Save();

            return result;
        }

        /// <summary>
        /// 保存通知内容
        /// </summary>
        /// <returns></returns>
        private async Task<object> Save()
        {
            var request = await HttpContext.Request.GetRequest();
            var callbackRequest = ObjectMapper.Map<Request, CallbackRequest>(request);
            callbackRequest.TenantId = CurrentTenant.Id;
            callbackRequest.CallbackType = CallbackType.PayNotify;

            var response = new JsonResponse { Code = System.Net.HttpStatusCode.OK };
            try
            {
                callbackRequest.HashCode = _signHelper.Sign(new
                {
                    callbackRequest.Url,
                    callbackRequest.Method,
                    callbackRequest.Body,
                    callbackRequest.Header
                }, nameof(CallbackRequest), separator: "&", containKey: true);

                var isNotify = await _callbackRequestRepository.AnyAsync(p => p.HashCode == callbackRequest.HashCode);
                CheckHelper.IsFalse(isNotify, "内容已通知，通知码：" + callbackRequest.HashCode);

                await _payManager.AssertNotify(new AssertNotifyRequest
                {
                    Body = callbackRequest.Body,
                    HashCode = callbackRequest.HashCode,
                    Headers = request.Headers,
                    Method = callbackRequest.Method,
                    Url = callbackRequest.Url,
                    PayTaskType = PayTaskType.AssertNotify,
                });
            }
            catch (Exception ex)
            {
                response.Code = System.Net.HttpStatusCode.BadRequest;
                response.Msg = ex.Message;
            }

            return response;
        }
    }
}
