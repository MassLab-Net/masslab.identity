using MassLab.Identity;
using MassLab.Identity.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MassLab.Identity.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(IdentityEntityFrameworkCoreModule),
    typeof(IdentityApplicationContractsModule)
)]
public class IdentityDbMigratorModule : AbpModule
{
}
