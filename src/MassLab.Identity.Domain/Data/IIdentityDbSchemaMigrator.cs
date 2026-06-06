using System.Threading.Tasks;

namespace MassLab.Identity.Data;

public interface IIdentityDbSchemaMigrator
{
    Task MigrateAsync();
}
