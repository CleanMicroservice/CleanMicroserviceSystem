namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients
{
    public class ClientUpdateRequest
    {
        public string? Name { get; set; }

        public bool? Enabled { get; set; }

        public string? Description { get; set; }

        public string? Secret { get; set; }
    }
}
