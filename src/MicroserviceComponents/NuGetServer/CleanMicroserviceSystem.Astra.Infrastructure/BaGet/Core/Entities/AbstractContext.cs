using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities.Converters;
using CleanMicroserviceSystem.Astra.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities
{
    public abstract class AbstractContext<TContext> : DbContext, IContext where TContext : DbContext
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
        protected readonly ILogger<AbstractContext<TContext>> logger;

        public AbstractContext(
            ILogger<AbstractContext<TContext>> logger)
            : base()
        {
            this.logger = logger;
        }

        public AbstractContext(
            ILogger<AbstractContext<TContext>> logger,
            DbContextOptions options)
            : base(options)
        {
            this.logger = logger;
        }

        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageDependency> PackageDependencies { get; set; }
        public DbSet<PackageType> PackageTypes { get; set; }
        public DbSet<TargetFramework> TargetFrameworks { get; set; }

        public Task<int> SaveChangesAsync() => SaveChangesAsync(default);

        public virtual async Task RunMigrationsAsync(CancellationToken cancellationToken)
            => await Database.MigrateAsync(cancellationToken);

        public abstract bool IsUniqueConstraintViolationException(DbUpdateException exception);

        public virtual bool SupportsLimitInSubqueries => true;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(log => this.logger.LogDebug(log));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Package>(BuildPackageEntity);
            builder.Entity<PackageDependency>(BuildPackageDependencyEntity);
            builder.Entity<PackageType>(BuildPackageTypeEntity);
            builder.Entity<TargetFramework>(BuildTargetFrameworkEntity);
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
                .HasMaxLength(MaxPackageVersionLength)
                .HasColumnType("TEXT COLLATE NOCASE")
                .IsRequired();

            package.Property(p => p.OriginalVersionString)
                .IsRequired(false)
                .HasMaxLength(MaxPackageVersionLength);

            package.Property(p => p.ReleaseNotes).IsRequired(false);

            package.Property(p => p.Authors)
                .IsRequired(false)
                .HasMaxLength(DefaultMaxStringLength)
                .HasConversion(StringArrayToJsonConverter.Instance)
                .Metadata.SetValueComparer(AstraStringArrayComparer.Instance);

            package.Property(p => p.IconUrl)
                .IsRequired(false)
                .HasConversion(UriToStringConverter.Instance)
                .HasMaxLength(DefaultMaxStringLength);

            package.Property(p => p.LicenseUrl)
                .IsRequired(false)
                .HasConversion(UriToStringConverter.Instance)
                .HasMaxLength(DefaultMaxStringLength);

            package.Property(p => p.ProjectUrl)
                .IsRequired(false)
                .HasConversion(UriToStringConverter.Instance)
                .HasMaxLength(DefaultMaxStringLength);

            package.Property(p => p.RepositoryUrl)
                .IsRequired(false)
                .HasConversion(UriToStringConverter.Instance)
                .HasMaxLength(DefaultMaxStringLength);

            package.Property(p => p.Tags)
                .IsRequired(false)
                .HasMaxLength(DefaultMaxStringLength)
                .HasConversion(StringArrayToJsonConverter.Instance)
                .Metadata.SetValueComparer(AstraStringArrayComparer.Instance);

            package.Property(p => p.Description).IsRequired(false).HasMaxLength(DefaultMaxStringLength);
            package.Property(p => p.Language).IsRequired(false).HasMaxLength(MaxPackageLanguageLength);
            package.Property(p => p.MinClientVersion).IsRequired(false).HasMaxLength(MaxPackageMinClientVersionLength);
            package.Property(p => p.Summary).IsRequired(false).HasMaxLength(DefaultMaxStringLength);
            package.Property(p => p.Title).IsRequired(false).HasMaxLength(MaxPackageTitleLength);
            package.Property(p => p.RepositoryType).IsRequired(false).HasMaxLength(MaxRepositoryTypeLength);

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

            package.Property(p => p.RowVersion).IsRequired(false).IsRowVersion();
        }

        private static void BuildPackageDependencyEntity(EntityTypeBuilder<PackageDependency> dependency)
        {
            dependency.HasKey(d => d.Key);
            dependency.HasIndex(d => d.Id);

            dependency.Property(d => d.Id).HasMaxLength(MaxPackageIdLength).HasColumnType("TEXT COLLATE NOCASE");
            dependency.Property(d => d.VersionRange).IsRequired(false).HasMaxLength(MaxPackageDependencyVersionRangeLength);
            dependency.Property(d => d.TargetFramework).IsRequired(false).HasMaxLength(MaxTargetFrameworkLength);
        }

        private static void BuildPackageTypeEntity(EntityTypeBuilder<PackageType> type)
        {
            type.HasKey(d => d.Key);
            type.HasIndex(d => d.Name);

            type.Property(d => d.Name).IsRequired(false).HasMaxLength(MaxPackageTypeNameLength).HasColumnType("TEXT COLLATE NOCASE");
            type.Property(d => d.Version).IsRequired(false).HasMaxLength(MaxPackageTypeVersionLength);
        }

        private static void BuildTargetFrameworkEntity(EntityTypeBuilder<TargetFramework> targetFramework)
        {
            targetFramework.HasKey(f => f.Key);
            targetFramework.HasIndex(f => f.Moniker);

            targetFramework.Property(f => f.Moniker).IsRequired(false).HasMaxLength(MaxTargetFrameworkLength).HasColumnType("TEXT COLLATE NOCASE");
        }
    }
}
