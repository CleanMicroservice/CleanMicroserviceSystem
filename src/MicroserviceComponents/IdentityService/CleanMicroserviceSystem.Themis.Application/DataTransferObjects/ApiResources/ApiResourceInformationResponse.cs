using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiScopes;

namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiResources
{
    public class ApiResourceInformationResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string? Description { get; set; }

        public virtual IEnumerable<ApiScopeInformationResponse>? ApiScopes { get; set; }
    }
}
