using BaGet.Core;
using CleanMicroserviceSystem.Astra.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanMicroserviceSystem.Astra.Infrastructure.Persistence.Configurations;

public static class BaGetConfiguration
{
    public const int DefaultMaxStringLength = 4000;
    public const int MaxPackageIdLength = 128;
    public const int MaxPackageVersionLength = 64;
    public const int MaxPackageMinClientVersionLength = 44;
    public const int MaxPackageLanguageLength = 20;
    public const int MaxPackageTitleLength = 256;
    public const int MaxPackageTypeNameLength = 512;
    public const int MaxPackageTypeVersionLength = 64;
    public const int MaxRepositoryTypeLength = 100;
    public const int MaxTargetFrameworkLength = 256;
    public const int MaxPackageDependencyVersionRangeLength = 256;

    public static ModelBuilder ConfigureBaGet(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Package>(BuildPackageEntity);
        modelBuilder.Entity<PackageDependency>(BuildPackageDependencyEntity);
        modelBuilder.Entity<PackageType>(BuildPackageTypeEntity);
        modelBuilder.Entity<TargetFramework>(BuildTargetFrameworkEntity);
        return modelBuilder;
    }

    private static void BuildPackageEntity(EntityTypeBuilder<Package> package)
    {
        package.HasKey(p => p.Key);
        package.HasIndex(p => p.Id);
        package.HasIndex(p => new { p.Id, p.NormalizedVersionString })
            .IsUnique();

        package.Property(p => p.Id)
            .HasMaxLength(MaxPackageIdLength)
            .HasColumnType("TEXT COLLATE NOCASE")
            .IsRequired();

        package.Property(p => p.NormalizedVersionString)
            .HasColumnName("Version")
            .HasMaxLength(MaxPackageVersionLength)
            .HasColumnType("TEXT COLLATE NOCASE")
            .IsRequired();

        package.Property(p => p.OriginalVersionString)
            .HasColumnName("OriginalVersion")
            .HasMaxLength(MaxPackageVersionLength);

        package.Property(p => p.ReleaseNotes)
            .HasColumnName("ReleaseNotes");

        package.Property(p => p.Authors)
            .HasMaxLength(DefaultMaxStringLength)
            .HasConversion(StringArrayToJsonConverter.Instance)
            .Metadata.SetValueComparer(AstraStringArrayComparer.Instance);

        package.Property(p => p.IconUrl)
            .HasConversion(UriToStringConverter.Instance)
            .HasMaxLength(DefaultMaxStringLength);

        package.Property(p => p.LicenseUrl)
            .HasConversion(UriToStringConverter.Instance)
            .HasMaxLength(DefaultMaxStringLength);

        package.Property(p => p.ProjectUrl)
            .HasConversion(UriToStringConverter.Instance)
            .HasMaxLength(DefaultMaxStringLength);

        package.Property(p => p.RepositoryUrl)
            .HasConversion(UriToStringConverter.Instance)
            .HasMaxLength(DefaultMaxStringLength);

        package.Property(p => p.Tags)
            .HasMaxLength(DefaultMaxStringLength)
            .HasConversion(StringArrayToJsonConverter.Instance)
            .Metadata.SetValueComparer(AstraStringArrayComparer.Instance);

        package.Property(p => p.Description).HasMaxLength(DefaultMaxStringLength);
        package.Property(p => p.Language).HasMaxLength(MaxPackageLanguageLength);
        package.Property(p => p.MinClientVersion).HasMaxLength(MaxPackageMinClientVersionLength);
        package.Property(p => p.Summary).HasMaxLength(DefaultMaxStringLength);
        package.Property(p => p.Title).HasMaxLength(MaxPackageTitleLength);
        package.Property(p => p.RepositoryType).HasMaxLength(MaxRepositoryTypeLength);

        package.Ignore(p => p.Version);
        package.Ignore(p => p.IconUrlString);
        package.Ignore(p => p.LicenseUrlString);
        package.Ignore(p => p.ProjectUrlString);
        package.Ignore(p => p.RepositoryUrlString);

        package.HasMany(p => p.PackageTypes)
            .WithOne(d => d.Package)
            .IsRequired();

        package.HasMany(p => p.TargetFrameworks)
            .WithOne(d => d.Package)
            .IsRequired();

        package.Property(p => p.RowVersion).IsRowVersion();
    }

    private static void BuildPackageDependencyEntity(EntityTypeBuilder<PackageDependency> dependency)
    {
        dependency.HasKey(d => d.Key);
        dependency.HasIndex(d => d.Id);

        dependency.Property(d => d.Id).HasMaxLength(MaxPackageIdLength).HasColumnType("TEXT COLLATE NOCASE");
        dependency.Property(d => d.VersionRange).HasMaxLength(MaxPackageDependencyVersionRangeLength);
        dependency.Property(d => d.TargetFramework).HasMaxLength(MaxTargetFrameworkLength);
    }

    private static void BuildPackageTypeEntity(EntityTypeBuilder<PackageType> type)
    {
        type.HasKey(d => d.Key);
        type.HasIndex(d => d.Name);

        type.Property(d => d.Name).HasMaxLength(MaxPackageTypeNameLength).HasColumnType("TEXT COLLATE NOCASE");
        type.Property(d => d.Version).HasMaxLength(MaxPackageTypeVersionLength);
    }

    private static void BuildTargetFrameworkEntity(EntityTypeBuilder<TargetFramework> targetFramework)
    {
        targetFramework.HasKey(f => f.Key);
        targetFramework.HasIndex(f => f.Moniker);

        targetFramework.Property(f => f.Moniker).HasMaxLength(MaxTargetFrameworkLength).HasColumnType("TEXT COLLATE NOCASE");
    }
}
