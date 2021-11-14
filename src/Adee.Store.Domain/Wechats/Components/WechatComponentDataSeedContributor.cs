using Adee.Store.Wechats.Components.Repositorys;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Adee.Store.Wechats.Components
{
    public class WechatComponentDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<WechatComponentConfig, Guid> _wechatComponentConfigRepository;
        private readonly IGuidGenerator _guidGenerator;

        public WechatComponentDataSeedContributor(
            IRepository<WechatComponentConfig, Guid> wechatComponentConfigRepository,
            IGuidGenerator guidGenerator
            )
        {
            _wechatComponentConfigRepository = wechatComponentConfigRepository;
            _guidGenerator = guidGenerator;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            await _wechatComponentConfigRepository.InsertAsync(new WechatComponentConfig(_guidGenerator.Create())
            {
                ComponentAppId = "wxa1609822f5093a1a",
                Token = "adee",
                EncodingAESKey = "adee123456adee123456adee123456adee123456789",
                Secret = "8cb872325aac675becd1e308324271f0",
            }, autoSave: true);
        }
    }
}
