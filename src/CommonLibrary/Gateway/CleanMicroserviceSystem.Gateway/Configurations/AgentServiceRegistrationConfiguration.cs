namespace CleanMicroserviceSystem.Gateway.Configurations;

public class AgentServiceRegistrationConfiguration
{
    public string ServicesProviderAddress { get; set; } = default!;

    public string ServiceName { get; set; } = default!;

    public string ServiceInstanceId { get; set; } = default!;

    public string SelfHost { get; set; } = default!;

    public int SelfPort { get; set; } = default!;

    public string HealthCheckUrl { get; set; } = default!;
}
