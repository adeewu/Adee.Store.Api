using Adee.Store.Wechats.OffiAccount.Messages.Repositorys;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Adee.Store.EntityFrameworkCore
{
    public partial class StoreDbContext
    {
        public virtual DbSet<WechatOffiAccoutReplyMessage> WechatOffiAccountReplyMessage { get; set; }

        public void ConfigureWechatOffiAccount(ModelBuilder builder)
        {
            builder.Entity<WechatOffiAccoutReplyMessage>(entity =>
            {
                entity.ToTable(StoreConsts.DbTablePrefix + nameof(WechatOffiAccountReplyMessage) + "s", StoreConsts.DbSchema);
                entity.ConfigureByConvention();

                entity.HasComment("公众平台被动回复消息");

                entity.Property(e => e.TenantId)
                    .HasComment("租户Id");

                entity.Property(e => e.Keyword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("关键字");

                entity.Property(e => e.MatchType)
                    .IsRequired()
                    .HasCommentForEnum("匹配类型");

                entity.Property(e => e.MessageType)
                    .HasMaxLength(10)
                    .HasComment("消息类型");

                entity.Property(e => e.MessageContent)
                    .HasComment("消息内容");

                entity.Property(e => e.IsDisabled)
                    .HasComment("禁用");
            });
        }
    }
}
