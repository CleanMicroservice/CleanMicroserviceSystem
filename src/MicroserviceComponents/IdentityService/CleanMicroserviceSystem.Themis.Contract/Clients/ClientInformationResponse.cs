namespace CleanMicroserviceSystem.Themis.Contract.Clients
{
    public class ClientInformationResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string? Description { get; set; }
    }
}
