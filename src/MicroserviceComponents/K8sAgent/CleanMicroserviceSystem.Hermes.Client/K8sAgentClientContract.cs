using CleanMicroserviceSystem.Gateway.Contract;

namespace CleanMicroserviceSystem.Hermes.Client;

public static class K8sAgentClientContract
{
    public const string HermesUriPrefix = $"{GatewayContract.GatewayUriPrefix}/Hermes";
}
