using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Adee.Store.Pays
{
    public interface IPayAppService
    {
        Task PayTask(PayTaskContentDto dto);
    }
}
