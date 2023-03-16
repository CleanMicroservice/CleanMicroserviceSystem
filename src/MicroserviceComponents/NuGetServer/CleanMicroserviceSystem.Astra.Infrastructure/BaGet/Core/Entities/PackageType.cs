namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities;

public class PackageType
{
    public int Key { get; set; }

    public string Name { get; set; }
    public string Version { get; set; }

    public virtual Package Package { get; set; }
}