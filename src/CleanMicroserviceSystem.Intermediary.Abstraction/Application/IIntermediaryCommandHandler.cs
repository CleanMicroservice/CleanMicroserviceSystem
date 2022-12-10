namespace CleanMicroserviceSystem.Intermediary.Abstraction.Application;

public interface IIntermediaryCommandHandler<TCommand> : IIntermediaryRequestHandler<TCommand, ValueTuple>
    where TCommand : IIntermediaryCommand
{
}
