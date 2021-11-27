using Adee.Store.Wechats.Components.Models;
using Adee.Store.Wechats.OffiAccount.Messages.Repositorys;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace Adee.Store.IdentityServer
{
    public class WechatOffiAccoutMessageDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly ITenantManager _tenantManager;
        private readonly IRepository<WechatOffiAccoutReplyMessage> _replyMessageRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public WechatOffiAccoutMessageDataSeedContributor(
            ITenantManager tenantManager,
            IRepository<WechatOffiAccoutReplyMessage> replyMessageRepository,
            ITenantRepository tenantRepository,
            IGuidGenerator guidGenerator,
            IUnitOfWork unitOfWork)
        {
            _tenantManager = tenantManager;
            _replyMessageRepository = replyMessageRepository;
            _tenantRepository = tenantRepository;
            _guidGenerator = guidGenerator;
            _unitOfWork = unitOfWork;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await CreateTestReplyMessage("OffiAccount-1");
            await CreateTestReplyMessage("OffiAccount-2");
            await CreateTestReplyMessage("OffiAccount-3");
            await CreateTestReplyMessage("OffiAccount-4");
            await CreateTestReplyMessage("OffiAccount-5");

            await CreateTestReplyMessage("MiniProgram-1");
            await CreateTestReplyMessage("MiniProgram-2");
            await CreateTestReplyMessage("MiniProgram-3");
            await CreateTestReplyMessage("MiniProgram-4");
            await CreateTestReplyMessage("MiniProgram-5");

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task CreateTestReplyMessage(string name)
        {
            var tenant = await _tenantRepository.FindByNameAsync(name);
            if (tenant.IsNotNull()) return;

            tenant = await _tenantManager.CreateAsync(name);
            tenant = await _tenantRepository.InsertAsync(tenant);

            await _replyMessageRepository.InsertAsync(new WechatOffiAccoutReplyMessage(_guidGenerator.Create())
            {
                Keyword = "TESTCOMPONENT_MSG_TYPE_TEXT",
                MatchType = Wechats.OffiAccount.MatchType.Full,
                MessageType = ReplyMessageType.Text,
                MessageContent = "TESTCOMPONENT_MSG_TYPE_TEXT_callback",
                TenantId = tenant.Id,
            });

            await _replyMessageRepository.InsertAsync(new WechatOffiAccoutReplyMessage(_guidGenerator.Create())
            {
                Keyword = "QUERY_AUTH_CODE:",
                MatchType = Wechats.OffiAccount.MatchType.StartLike,
                MessageType = ReplyMessageType.CustomerService,
                TenantId = tenant.Id,
            });
        }
    }
}
