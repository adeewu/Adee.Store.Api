using Adee.Store.Attributes;
using Adee.Store.Domain.Shared.Utils.Helpers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
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
    public class NotifyAppService : StoreWithRequestAppService, INotifyAppService, ITransientDependency
    {
        private readonly SignHelper _signHelper;
        private readonly IRepository<PayNotify> _payNotifyRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly PayManager _payManager;

        public NotifyAppService(
            SignHelper signHelper,
            IRepository<PayNotify> payNotifyRepository,
            IRepository<Tenant> tenantRepository,
            PayManager payManager
            )
        {
            _signHelper = signHelper;
            _payNotifyRepository = payNotifyRepository;
            _tenantRepository = tenantRepository;
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

            var result = await Save(new NotifyDto
            {
                Method = HttpMethod.Get,
                Query = HttpContext.Request.QueryString.HasValue ? HttpContext.Request.QueryString.Value : string.Empty,
                Url = HttpContext.Request.GetDisplayUrl(),
                TenantId = CurrentTenant.Id,
                TargetDomain = CurrentDomain,
            });

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

            var body = await HttpContext.Request.GetBodyAsync();

            var result = await Save(new NotifyDto
            {
                Method = HttpMethod.Post,
                Body = body,
                Query = HttpContext.Request.QueryString.HasValue ? HttpContext.Request.QueryString.Value : string.Empty,
                Url = HttpContext.Request.GetDisplayUrl(),
                TenantId = CurrentTenant.Id,
                TargetDomain = CurrentDomain,
            });

            return result;
        }

        /// <summary>
        /// 保存通知内容
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<object> Save(NotifyDto dto)
        {
            var existTenant = await _tenantRepository.AnyAsync(p => p.Id == dto.TenantId);
            CheckHelper.IsTrue(existTenant, $"租户Id：{dto.TenantId} 无效");

            var response = new JsonResponse { Code = System.Net.HttpStatusCode.OK };
            try
            {
                var hashCode = _signHelper.Sign(new
                {
                    dto.Url,
                    dto.Method,
                    dto.Query,
                    dto.Body,
                }, nameof(NotifyDto), separator: "&", containKey: true);

                var isNotify = await _payNotifyRepository.AnyAsync(p => p.HashCode == hashCode);
                CheckHelper.IsFalse(isNotify, "内容已通知，通知码：" + hashCode);

                var notify = new PayNotify
                {
                    Body = dto.Body,
                    Method = dto.Method.Method,
                    Query = dto.Query,
                    Url = dto.Url,
                    HashCode = hashCode,
                    TenantId = dto.TenantId,
                    Status = PayTaskStatus.Normal,
                };
                await _payManager.AssertNotify(notify);
            }
            catch (Exception ex)
            {
                response.Code = System.Net.HttpStatusCode.BadRequest;
                response.Msg = ex.Message;
            }

            //根据通道处理返回值

            return response;
        }
    }
}
