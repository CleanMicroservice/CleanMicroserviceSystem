namespace CleanMicroserviceSystem.Intermediary.Abstraction.Application;

public interface IIntermediaryNotificationHandler<in TNotification> : INotificationHandler<TNotification>
    where TNotification : IIntermediaryNotification
{
}
