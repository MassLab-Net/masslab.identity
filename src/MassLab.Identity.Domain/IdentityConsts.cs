using Volo.Abp.Identity;

namespace MassLab.Identity;

public static class IdentityConsts
{
    public const string DbTablePrefix = "App";
    public const string DbSchema = "identity";
    public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = "1q2w3E*";
}
