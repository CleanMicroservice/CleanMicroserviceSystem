namespace CleanMicroserviceSystem.Intermediary.Abstraction.Domain;

public interface IIntermediaryRequest<out TResponse> : IRequest<TResponse>
{
}
