namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiScopes
{
    public class ApiScopeInformationResponse
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string? Description { get; set; }
    }
}
