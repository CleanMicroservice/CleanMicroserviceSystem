namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.Clients
{
    public class ClientInformationResponse
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string? Description { get; set; }
    }
}
