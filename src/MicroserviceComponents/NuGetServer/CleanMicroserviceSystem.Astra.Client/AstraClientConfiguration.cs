using CleanMicroserviceSystem.Oceanus.Client.Abstraction;

namespace CleanMicroserviceSystem.Astra.Client;

public class AstraClientConfiguration : OceanusServiceClientConfiguration
{
    public string ApiKey { get; set; }
}
