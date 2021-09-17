using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Pays
{
    public interface INotifyAppService : ITransientDependency
    {
        /// <summary>
        /// 保存通知内容
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<object> Save(NotifyDto dto);
    }
}
