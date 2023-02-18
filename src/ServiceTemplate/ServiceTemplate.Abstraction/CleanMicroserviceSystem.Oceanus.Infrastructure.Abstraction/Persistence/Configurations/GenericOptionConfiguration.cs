using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanMicroserviceSystem.Oceanus.Infrastructure.Abstraction.Persistence.Configurations;

internal static class GenericOptionConfiguration
{
    public static ModelBuilder ConfigureGenericOption(this ModelBuilder modelBuilder)
    {
        var entityBuilder = modelBuilder.Entity<GenericOption>();
        entityBuilder.HasKey(nameof(WebAPILog.Id));
        entityBuilder
            .HasIndex(
                nameof(GenericOption.OptionName),
                nameof(GenericOption.Category),
                nameof(GenericOption.OwnerLevel))
            .IsUnique();
        entityBuilder.Property(nameof(GenericOption.OptionName)).UseCollation("NOCASE");
        entityBuilder.Property(nameof(GenericOption.Category)).UseCollation("NOCASE");
        entityBuilder.Property(nameof(GenericOption.OwnerLevel)).UseCollation("NOCASE");
        return modelBuilder;
    }
}
