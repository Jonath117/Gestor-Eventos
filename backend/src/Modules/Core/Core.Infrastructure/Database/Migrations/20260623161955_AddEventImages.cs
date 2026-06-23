using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEventImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cover_image_url",
                schema: "core",
                table: "events",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_qr_image_url",
                schema: "core",
                table: "events",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cover_image_url",
                schema: "core",
                table: "events");

            migrationBuilder.DropColumn(
                name: "payment_qr_image_url",
                schema: "core",
                table: "events");
        }
    }
}
