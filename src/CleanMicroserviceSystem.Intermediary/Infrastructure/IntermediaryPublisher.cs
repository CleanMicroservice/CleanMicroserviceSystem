namespace CleanMicroserviceSystem.Intermediary.Infrastructure;

public class IntermediaryPublisher : IIntermediaryPublisher
{
    private readonly ILogger<IntermediaryPublisher> logger;
    private readonly IMediator mediator;

    public IntermediaryPublisher(
        ILogger<IntermediaryPublisher> logger,
        IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task SendCommand(IIntermediaryCommand command, CancellationToken cancellationToken = default)
    {
        logger.LogDebug($"Sending command of type {command?.GetType()?.FullName ?? "[Null]"}...");
        _ = await mediator.Send(command, cancellationToken);
    }

    public async Task<TResponse> SendRequest<TResponse>(IIntermediaryRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        logger.LogDebug($"Sending request of type {request?.GetType()?.FullName ?? "[Null]"}...");
        var response = await mediator.Send(request, cancellationToken);
        logger.LogDebug($"Received response of type {response?.GetType()?.FullName ?? "[Null]"}.");
        return response;
    }

    public async Task PublishNotification(IIntermediaryNotification notification, CancellationToken cancellationToken = default)
    {
        logger.LogDebug($"Publishing notification type of {notification.GetType().FullName}...");
        await mediator.Publish(notification, cancellationToken);
    }

    public async Task PublishEvent(IIntermediaryEvent eventArg, CancellationToken cancellationToken = default)
    {
        logger.LogDebug($"Publishing event type of {eventArg.GetType().FullName}...");
        await mediator.Publish(eventArg, cancellationToken);
    }
}
