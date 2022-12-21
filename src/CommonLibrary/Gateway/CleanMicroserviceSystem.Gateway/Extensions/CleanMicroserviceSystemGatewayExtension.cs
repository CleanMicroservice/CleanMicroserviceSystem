using CleanMicroserviceSystem.Gateway.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMicroserviceSystem.Gateway.Extensions
{
    public static class CleanMicroserviceSystemGatewayExtension
    {
        public static IServiceCollection AddJwtBearerAuthentication(
            this IServiceCollection services,
            ConsulConfiguration configuration)
        {
            services
                .Configure(new Action<ConsulConfiguration>(options =>
                {
                    options.Address = configuration.Address;
                }));

            return services;
        }
    }
}
