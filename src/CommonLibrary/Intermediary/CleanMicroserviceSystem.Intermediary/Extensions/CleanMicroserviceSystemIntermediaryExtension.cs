namespace CleanMicroserviceSystem.Intermediary.Extensions;

public static class CleanMicroserviceSystemIntermediaryExtension
{
    public static IServiceCollection AddCleanMicroserviceSystemIntermediary(this IServiceCollection services)
        => services
            .AddMediatR(typeof(CleanMicroserviceSystemIntermediaryExtension).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(IntermediaryPipelineBehavior<,>))
            .AddTransient<IIntermediaryPublisher, IntermediaryPublisher>();
}
