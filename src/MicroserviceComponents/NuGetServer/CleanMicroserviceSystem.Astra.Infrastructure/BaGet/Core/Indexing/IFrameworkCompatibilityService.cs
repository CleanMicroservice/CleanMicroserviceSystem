namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;

public interface IFrameworkCompatibilityService
{
    IReadOnlyList<string> FindAllCompatibleFrameworks(string framework);
}