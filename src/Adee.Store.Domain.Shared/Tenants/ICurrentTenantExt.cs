using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Domain.Tenants
{
    /// <summary>
    /// 租户扩展信息
    /// </summary>
    public interface ICurrentTenantExt : ITransientDependency
    {
        /// <summary>
        /// 软件编号
        /// </summary>
        Task<string> GetSoftwareCodeAsync();
    }
}
