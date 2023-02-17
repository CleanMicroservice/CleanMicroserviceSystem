namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients
{
    public class ClientSearchRequest
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public bool? Enabled { get; set; }
        public int Start { get; set; }
        public int Count { get; set; }
    }
}
