namespace CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiResources
{
    public class ApiResourceInformationResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string? Description { get; set; }
    }
}
