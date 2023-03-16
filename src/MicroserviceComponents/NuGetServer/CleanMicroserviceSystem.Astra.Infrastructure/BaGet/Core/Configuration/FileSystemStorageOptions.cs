using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Configuration;

public class FileSystemStorageOptions : IValidatableObject
{
    public string Path { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(this.Path))
            this.Path = Directory.GetCurrentDirectory();

        return Enumerable.Empty<ValidationResult>();
    }
}