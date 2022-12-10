namespace CleanMicroserviceSystem.Intermediary.Abstraction.Application;

public class DefaultIntermediaryEventHandler<TEvent> : IIntermediaryEventHandler<TEvent>
    where TEvent : IIntermediaryEvent
{
    public event EventHandler<TEvent> EventRaised;

    public async Task Handle(TEvent eventArg, CancellationToken cancellationToken)
    {
        EventRaised?.Invoke(this, eventArg);
    }
}
