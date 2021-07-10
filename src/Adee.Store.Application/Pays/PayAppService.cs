using Adee.Store.Pays;
using Adee.Store.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;

namespace Adee.Store.Products
{
    /// <summary>
    /// 支付服务
    /// </summary>    
    public class PayAppService : ApplicationService
    {
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly IRepository<PayOrder> _payOrderRepository;
        private readonly IReadOnlyRepository<PayParameter> _payParameterRepository;

        public PayAppService(
            IDistributedEventBus distributedEventBus,
            IRepository<PayOrder> payOrderRepository,
            IReadOnlyRepository<PayParameter> payParameterRepository)
        {
            _distributedEventBus = distributedEventBus;
            _payOrderRepository = payOrderRepository;
            _payParameterRepository = payParameterRepository;
        }

        public async Task PayTask(PayTaskContentDto dto)
        {
            await Task.CompletedTask;
        }

        private async Task PayTask2(PayTaskContentDto dto, PaymentType paymentType)
        {
            var isDuplicateMerchantOrderId = await IsDuplicateMerchantOrderId(dto.MerchantOrderId);
            if (isDuplicateMerchantOrderId) throw new UserFriendlyException($"{dto.MerchantOrderId}已经使用");

            var payParameter = await GetPayParameter(paymentType);

            // await _payOrderRepository.InsertAsync(new PayOrder(GuidGenerator.Create())
            // {
            //     Money = view.Money,
            //     PayOrderId = payView.PayOrderId,
            //     MerchantOrderId = dto.MerchantOrderId,
            //     TenantId = CurrentTenant.Id,
            //     ParameterVersion = payParameter.Version,
            //     PaymentType = paymentType,
            //     PayOrganizationType = payParameter.PayOrganizationType,
            //     Status = PayTaskStatus.Normal,
            //     StatusMessage = PayTaskStatus.Normal.GetDescription(),
            //     OrderData = view.AuthCode,
            //     PaymethodType = PaymethodType.B2C,
            //     NotifyUrl = view.NotifyUrl,
            //     TargetDomain = domain,
            //     BusinessType = view.BusinessType,
            //     Subject = view.Subject,
            // });

            await _distributedEventBus.PublishAsync(dto);
        }

        public async Task<PayParameter> GetPayParameter(PaymentType paymentType, int? version = null)
        {
            if (!version.HasValue)
            {
                version = await AsyncExecuter.MaxAsync(_payParameterRepository.Where(p => p.TenantId == CurrentTenant.Id), p => p.Version);
            }
            Check.NotNull(version, nameof(version));

            var parameters = await AsyncExecuter.ToListAsync(_payParameterRepository.Where(p => p.TenantId == CurrentTenant.Id).Where(p => p.Version == version));
            Check.NotNullOrEmpty(parameters, nameof(parameters));

            var hitParameters = parameters.Where(p => p.PaymentType == paymentType);
            if (hitParameters.Count() > 1) throw new UserFriendlyException($"数据错误，版本：{version}包含{hitParameters.Count()}个支付参数");
            if (hitParameters.Count() == 1) return hitParameters.FirstOrDefault();

            if (parameters.Any(p => !p.PaymentType.HasValue))
            {
                var hitPayParameter = parameters.FirstOrDefault();
                hitPayParameter.PaymentType = paymentType;

                return hitPayParameter;
            }

            throw new UserFriendlyException($"未查找到版本：{version}的支付参数");
        }

        private async Task<bool> IsDuplicateMerchantOrderId(string merchantOrderId)
        {
            return await AsyncExecuter.AnyAsync(_payOrderRepository, p => p.MerchantOrderId == merchantOrderId);
        }

    }
}
