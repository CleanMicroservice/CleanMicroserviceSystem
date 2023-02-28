namespace CleanMicroserviceSystem.Gateway.Configurations;

public class AgentServiceRegistrationConfiguration
{
    public string ServicesProviderAddress { get; set; }

    public string ServiceName { get; set; }

    public string ServiceInstanceId { get; set; }

    public string SelfHost { get; set; }

    public int SelfPort { get; set; }

    public string HealthCheckUrl { get; set; }
}
