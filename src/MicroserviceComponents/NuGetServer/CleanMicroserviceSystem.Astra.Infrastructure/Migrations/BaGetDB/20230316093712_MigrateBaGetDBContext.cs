using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanMicroserviceSystem.Astra.Infrastructure.Migrations.BaGetDB;

/// <inheritdoc />
public partial class MigrateBaGetDBContext : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "Packages",
            columns: table => new
            {
                Key = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Id = table.Column<string>(type: "TEXT COLLATE NOCASE", maxLength: 128, nullable: false),
                Authors = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                Description = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                Downloads = table.Column<long>(type: "INTEGER", nullable: false),
                HasReadme = table.Column<bool>(type: "INTEGER", nullable: false),
                HasEmbeddedIcon = table.Column<bool>(type: "INTEGER", nullable: false),
                IsPrerelease = table.Column<bool>(type: "INTEGER", nullable: false),
                ReleaseNotes = table.Column<string>(type: "TEXT", nullable: true),
                Language = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                Listed = table.Column<bool>(type: "INTEGER", nullable: false),
                MinClientVersion = table.Column<string>(type: "TEXT", maxLength: 44, nullable: true),
                Published = table.Column<DateTime>(type: "TEXT", nullable: false),
                RequireLicenseAcceptance = table.Column<bool>(type: "INTEGER", nullable: false),
                SemVerLevel = table.Column<int>(type: "INTEGER", nullable: false),
                Summary = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                IconUrl = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                LicenseUrl = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                ProjectUrl = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                RepositoryUrl = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                RepositoryType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                Tags = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                NormalizedVersionString = table.Column<string>(type: "TEXT COLLATE NOCASE", maxLength: 64, nullable: false),
                OriginalVersionString = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_Packages", x => x.Key);
            });

        _ = migrationBuilder.CreateTable(
            name: "PackageDependencies",
            columns: table => new
            {
                Key = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Id = table.Column<string>(type: "TEXT COLLATE NOCASE", maxLength: 128, nullable: true),
                VersionRange = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                TargetFramework = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                PackageKey = table.Column<int>(type: "INTEGER", nullable: true)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_PackageDependencies", x => x.Key);
                _ = table.ForeignKey(
                    name: "FK_PackageDependencies_Packages_PackageKey",
                    column: x => x.PackageKey,
                    principalTable: "Packages",
                    principalColumn: "Key");
            });

        _ = migrationBuilder.CreateTable(
            name: "PackageTypes",
            columns: table => new
            {
                Key = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT COLLATE NOCASE", maxLength: 512, nullable: true),
                Version = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                PackageKey = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_PackageTypes", x => x.Key);
                _ = table.ForeignKey(
                    name: "FK_PackageTypes_Packages_PackageKey",
                    column: x => x.PackageKey,
                    principalTable: "Packages",
                    principalColumn: "Key",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateTable(
            name: "TargetFrameworks",
            columns: table => new
            {
                Key = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Moniker = table.Column<string>(type: "TEXT COLLATE NOCASE", maxLength: 256, nullable: true),
                PackageKey = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                _ = table.PrimaryKey("PK_TargetFrameworks", x => x.Key);
                _ = table.ForeignKey(
                    name: "FK_TargetFrameworks_Packages_PackageKey",
                    column: x => x.PackageKey,
                    principalTable: "Packages",
                    principalColumn: "Key",
                    onDelete: ReferentialAction.Cascade);
            });

        _ = migrationBuilder.CreateIndex(
            name: "IX_PackageDependencies_Id",
            table: "PackageDependencies",
            column: "Id");

        _ = migrationBuilder.CreateIndex(
            name: "IX_PackageDependencies_PackageKey",
            table: "PackageDependencies",
            column: "PackageKey");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Packages_Id",
            table: "Packages",
            column: "Id");

        _ = migrationBuilder.CreateIndex(
            name: "IX_Packages_Id_NormalizedVersionString",
            table: "Packages",
            columns: new[] { "Id", "NormalizedVersionString" },
            unique: true);

        _ = migrationBuilder.CreateIndex(
            name: "IX_PackageTypes_Name",
            table: "PackageTypes",
            column: "Name");

        _ = migrationBuilder.CreateIndex(
            name: "IX_PackageTypes_PackageKey",
            table: "PackageTypes",
            column: "PackageKey");

        _ = migrationBuilder.CreateIndex(
            name: "IX_TargetFrameworks_Moniker",
            table: "TargetFrameworks",
            column: "Moniker");

        _ = migrationBuilder.CreateIndex(
            name: "IX_TargetFrameworks_PackageKey",
            table: "TargetFrameworks",
            column: "PackageKey");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropTable(
            name: "PackageDependencies");

        _ = migrationBuilder.DropTable(
            name: "PackageTypes");

        _ = migrationBuilder.DropTable(
            name: "TargetFrameworks");

        _ = migrationBuilder.DropTable(
            name: "Packages");
    }
}
