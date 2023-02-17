using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanMicroserviceSystem.Themis.Infrastructure.Migrations.ConfigurationDb
{
    /// <inheritdoc />
    public partial class MigrateConfigurationDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiResources",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "INTEGER", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResources", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Secret = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "INTEGER", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ApiScopes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ApiResourceID = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "INTEGER", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ApiScopes_ApiResources_ApiResourceID",
                        column: x => x.ApiResourceID,
                        principalTable: "ApiResources",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientApiScopeMaps",
                columns: table => new
                {
                    ClientID = table.Column<int>(type: "INTEGER", nullable: false),
                    ApiScopeID = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "INTEGER", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientApiScopeMaps", x => new { x.ClientID, x.ApiScopeID });
                    table.ForeignKey(
                        name: "FK_ClientApiScopeMaps_ApiScopes_ApiScopeID",
                        column: x => x.ApiScopeID,
                        principalTable: "ApiScopes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientApiScopeMaps_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApiResources",
                columns: new[] { "ID", "CreatedBy", "CreatedOn", "Description", "Enabled", "LastModifiedBy", "LastModifiedOn", "Name" },
                values: new object[] { 1, 1, new DateTime(2023, 2, 17, 6, 53, 34, 254, DateTimeKind.Utc).AddTicks(4760), "ThemisAPI", false, null, null, "ThemisAPI" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ID", "CreatedBy", "CreatedOn", "Description", "Enabled", "LastModifiedBy", "LastModifiedOn", "Name", "Secret" },
                values: new object[] { 1, 1, new DateTime(2023, 2, 17, 6, 53, 34, 254, DateTimeKind.Utc).AddTicks(4866), "Tethys", false, null, null, "Tethys", "dZ4LIKrWTu4W+XlkYYEamdddV4MrXnxZpjPUQClKn+8=" });

            migrationBuilder.InsertData(
                table: "ApiScopes",
                columns: new[] { "ID", "ApiResourceID", "CreatedBy", "CreatedOn", "Description", "Enabled", "LastModifiedBy", "LastModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2023, 2, 17, 6, 53, 34, 254, DateTimeKind.Utc).AddTicks(4836), "ThemisAPI.Read", false, null, null, "ThemisAPI.Read" },
                    { 2, 1, 1, new DateTime(2023, 2, 17, 6, 53, 34, 254, DateTimeKind.Utc).AddTicks(4838), "ThemisAPI.Write", false, null, null, "ThemisAPI.Write" }
                });

            migrationBuilder.InsertData(
                table: "ClientApiScopeMaps",
                columns: new[] { "ApiScopeID", "ClientID", "CreatedBy", "CreatedOn", "LastModifiedBy", "LastModifiedOn" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2023, 2, 17, 6, 53, 34, 254, DateTimeKind.Utc).AddTicks(5151), null, null },
                    { 2, 1, 1, new DateTime(2023, 2, 17, 6, 53, 34, 254, DateTimeKind.Utc).AddTicks(5152), null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiResources_Name",
                table: "ApiResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiScopes_ApiResourceID_Name",
                table: "ApiScopes",
                columns: new[] { "ApiResourceID", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientApiScopeMaps_ApiScopeID",
                table: "ClientApiScopeMaps",
                column: "ApiScopeID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Name",
                table: "Clients",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientApiScopeMaps");

            migrationBuilder.DropTable(
                name: "ApiScopes");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "ApiResources");
        }
    }
}
