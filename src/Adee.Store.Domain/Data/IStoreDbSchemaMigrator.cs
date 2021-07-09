using System.Threading.Tasks;

namespace Adee.Store.Data
{
    public interface IStoreDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
