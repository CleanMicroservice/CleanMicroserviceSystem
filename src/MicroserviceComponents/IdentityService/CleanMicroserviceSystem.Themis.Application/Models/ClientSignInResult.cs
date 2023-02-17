using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Application.Models
{
    public class ClientSigninResult
    {
        public ClientSigninResult(Client? client = null)
        {
            this.Client = client;
        }

        public Client? Client { get; private set; }

        public bool Success { get => string.IsNullOrEmpty(this.Error); }

        public string? Error { get; set; }
    }
}
