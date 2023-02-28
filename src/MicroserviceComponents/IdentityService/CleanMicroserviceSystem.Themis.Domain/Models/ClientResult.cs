using CleanMicroserviceSystem.Themis.Domain.Entities.Configuration;

namespace CleanMicroserviceSystem.Themis.Domain.Models;

public class ClientResult
{
    public Client? Client { get; set; }

    public bool Succeeded => string.IsNullOrEmpty(this.Error);

    public string? Error { get; set; }
}
