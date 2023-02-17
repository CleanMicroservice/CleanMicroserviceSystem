using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiResources;

namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients
{
    public class ClientInformationResponse
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string? Description { get; set; }
    }
}
