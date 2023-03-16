using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;

public class DatabaseOptions
{
    public string Type { get; set; }

    [Required]
    public string ConnectionString { get; set; }
}