namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients
{
    public class ApiResourceUpdateRequest
    {
        public string? Name { get; set; }

        public bool? Enabled { get; set; }

        public string? Description { get; set; }
    }
}
