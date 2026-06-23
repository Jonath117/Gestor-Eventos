using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "core",
                table: "users",
                columns: new[] { "id", "created_at", "email", "password_hash" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 6, 23, 5, 49, 39, 955, DateTimeKind.Utc).AddTicks(7842), "admin@campeando.com", "$2a$11$MGpEBiAytwzYQQZ/23CWRueAnvvyrFHumKraeMObdiqdEDOiP8FlG" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "core",
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
