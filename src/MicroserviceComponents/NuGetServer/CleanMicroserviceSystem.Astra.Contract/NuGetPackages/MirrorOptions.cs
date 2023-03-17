using System.ComponentModel.DataAnnotations;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages;

public class MirrorOptions : IValidatableObject
{
    public bool Enabled { get; set; }

    public Uri PackageSource { get; set; }

    public bool Legacy { get; set; }

    [Range(0, int.MaxValue)]
    public int PackageDownloadTimeoutSeconds { get; set; } = 600;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (this.Enabled && this.PackageSource == null)
        {
            yield return new ValidationResult(
                $"The {nameof(this.PackageSource)} configuration is required if mirroring is enabled",
                new[] { nameof(this.PackageSource) });
        }
    }
}