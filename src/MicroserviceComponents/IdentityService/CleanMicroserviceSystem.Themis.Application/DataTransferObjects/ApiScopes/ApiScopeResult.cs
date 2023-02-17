using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiResources
{
    public class ApiScopeResult
    {
        public ApiScope? ApiScope { get; set; }

        public bool Succeeded { get => string.IsNullOrEmpty(Error); }

        public string? Error { get; set; }
    }
}
