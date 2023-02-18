using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence.Configurations;

internal static class WebAPILogConfiguration
{
    public static ModelBuilder ConfigureWebAPILog(this ModelBuilder modelBuilder)
    {
        var entityBuilder = modelBuilder.Entity<WebAPILog>();
        entityBuilder.HasKey(nameof(WebAPILog.Id));
        entityBuilder.HasIndex(
                nameof(WebAPILog.RequestURI),
                nameof(WebAPILog.SourceHost),
                nameof(WebAPILog.IdentityName),
                nameof(WebAPILog.CreatedOn));
        return modelBuilder;
    }
}
