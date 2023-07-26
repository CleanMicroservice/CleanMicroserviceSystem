using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanMicroserviceSystem.Themis.Infrastructure.Migrations.ConfigurationDb
{
    /// <inheritdoc />
    public partial class InitializeThemisNTLMClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2023, 7, 26, 7, 7, 27, 133, DateTimeKind.Utc).AddTicks(2484));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2023, 7, 26, 7, 7, 27, 133, DateTimeKind.Utc).AddTicks(2547));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2023, 4, 24, 12, 34, 15, 993, DateTimeKind.Utc).AddTicks(8880));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2023, 4, 24, 12, 34, 15, 993, DateTimeKind.Utc).AddTicks(8952));
        }
    }
}
