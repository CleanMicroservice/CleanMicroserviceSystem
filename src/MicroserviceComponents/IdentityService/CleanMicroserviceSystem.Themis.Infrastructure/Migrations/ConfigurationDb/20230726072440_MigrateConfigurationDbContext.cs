﻿using System;
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
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
                    table.PrimaryKey("PK_ApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
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
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientClaims_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApiResources",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Description", "Enabled", "LastModifiedBy", "LastModifiedOn", "Name" },
                values: new object[] { 1, 1, new DateTime(2023, 7, 26, 7, 24, 40, 601, DateTimeKind.Utc).AddTicks(3292), "ThemisAPI", true, null, null, "ThemisAPI" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Description", "Enabled", "LastModifiedBy", "LastModifiedOn", "Name", "Secret" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 7, 26, 7, 24, 40, 601, DateTimeKind.Utc).AddTicks(3362), "Tethys", true, null, null, "Tethys", "dZ4LIKrWTu4W+XlkYYEamdddV4MrXnxZpjPUQClKn+8=" },
                    { 2, 1, new DateTime(2023, 7, 26, 7, 24, 40, 601, DateTimeKind.Utc).AddTicks(3468), "ThemisNTLM", true, null, null, "ThemisNTLM", "SvFQgEUNsTWoB4JUGbTUyfr9sh9bwFDf3inRqrwEDMs=" }
                });

            migrationBuilder.InsertData(
                table: "ClientClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "ClientId" },
                values: new object[,]
                {
                    { 1, "ThemisAPI", "Read", 1 },
                    { 2, "ThemisAPI", "Write", 1 },
                    { 3, "ThemisAPI", "Read", 2 },
                    { 4, "ThemisAPI", "Write", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiResources_Name",
                table: "ApiResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientClaims_ClaimType_ClaimValue",
                table: "ClientClaims",
                columns: new[] { "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientClaims_ClientId",
                table: "ClientClaims",
                column: "ClientId");

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
                name: "ApiResources");

            migrationBuilder.DropTable(
                name: "ClientClaims");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
