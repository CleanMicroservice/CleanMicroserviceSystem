namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;

public class PackageDependency
{
    public int Key { get; set; }

    public string Id { get; set; }

    public string VersionRange { get; set; }

    public string TargetFramework { get; set; }

    public virtual Package Package { get; set; }
}