namespace CleanMicroserviceSystem.Intermediary.Abstraction.Application;

public interface IIntermediaryEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IIntermediaryEvent
{
    event EventHandler<TEvent> EventRaised;
}
