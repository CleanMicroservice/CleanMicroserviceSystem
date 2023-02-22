using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Domain.DTOs.Clients
{
    public class ClientResult
    {
        public Client? Client { get; set; }

        public bool Succeeded { get => string.IsNullOrEmpty(Error); }

        public string? Error { get; set; }
    }
}
