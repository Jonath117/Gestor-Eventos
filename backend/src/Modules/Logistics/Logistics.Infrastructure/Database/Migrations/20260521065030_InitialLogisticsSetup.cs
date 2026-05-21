using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialLogisticsSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "logistics");

            migrationBuilder.CreateTable(
                name: "offline_sync_projections",
                schema: "logistics",
                columns: table => new
                {
                    participant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qr_identifier = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_offline_sync_projections", x => x.participant_id);
                });

            migrationBuilder.CreateTable(
                name: "ration_configs",
                schema: "logistics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    total_allowed_per_participant = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ration_configs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "check_ins",
                schema: "logistics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    participant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ration_config_id = table.Column<Guid>(type: "uuid", nullable: true),
                    scanned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    offline_sync_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_check_ins", x => x.id);
                    table.ForeignKey(
                        name: "fk_check_ins_ration_configs_ration_config_id",
                        column: x => x.ration_config_id,
                        principalSchema: "logistics",
                        principalTable: "ration_configs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                schema: "logistics",
                table: "check_ins",
                columns: new[] { "id", "event_id", "offline_sync_id", "organization_id", "participant_id", "ration_config_id", "scanned_at" },
                values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("99999999-9999-9999-9999-999999999999"), "sync_001_mobile", new Guid("11111111-1111-1111-1111-111111111111"), new Guid("33333333-3333-3333-3333-333333333333"), null, new DateTime(2026, 5, 20, 8, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                schema: "logistics",
                table: "offline_sync_projections",
                columns: new[] { "participant_id", "event_id", "full_name", "is_confirmed", "last_updated_at", "qr_identifier" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("99999999-9999-9999-9999-999999999999"), "Jonathan Rocha", true, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "HASH_QR_SEGURO_001" });

            migrationBuilder.InsertData(
                schema: "logistics",
                table: "ration_configs",
                columns: new[] { "id", "event_id", "name", "organization_id", "total_allowed_per_participant" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("99999999-9999-9999-9999-999999999999"), "Almuerzo Día 1", new Guid("11111111-1111-1111-1111-111111111111"), 1 });

            migrationBuilder.CreateIndex(
                name: "ix_check_ins_offline_sync_id",
                schema: "logistics",
                table: "check_ins",
                column: "offline_sync_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_check_ins_ration_config_id",
                schema: "logistics",
                table: "check_ins",
                column: "ration_config_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "check_ins",
                schema: "logistics");

            migrationBuilder.DropTable(
                name: "offline_sync_projections",
                schema: "logistics");

            migrationBuilder.DropTable(
                name: "ration_configs",
                schema: "logistics");
        }
    }
}
