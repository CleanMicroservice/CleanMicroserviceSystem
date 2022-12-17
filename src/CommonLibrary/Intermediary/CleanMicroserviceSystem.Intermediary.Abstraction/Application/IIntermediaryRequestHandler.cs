namespace CleanMicroserviceSystem.Intermediary.Abstraction.Application;

public interface IIntermediaryRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IIntermediaryRequest<TResponse>
{
}
