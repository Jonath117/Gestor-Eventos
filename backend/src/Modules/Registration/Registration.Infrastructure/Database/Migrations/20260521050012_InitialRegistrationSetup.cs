using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registration.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialRegistrationSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "registration");

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "registration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_email = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "participants",
                schema: "registration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true),
                    qr_identifier = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_participants", x => x.id);
                    table.ForeignKey(
                        name: "fk_participants_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "registration",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "codes",
                schema: "registration",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    used_by_participant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    used_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_codes", x => x.id);
                    table.ForeignKey(
                        name: "fk_codes_participants_used_by_participant_id",
                        column: x => x.used_by_participant_id,
                        principalSchema: "registration",
                        principalTable: "participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                schema: "registration",
                table: "codes",
                columns: new[] { "id", "event_id", "organization_id", "token", "used_at", "used_by_participant_id" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("99999999-9999-9999-9999-999999999999"), new Guid("11111111-1111-1111-1111-111111111111"), "BECA-UCB-100", null, null });

            migrationBuilder.InsertData(
                schema: "registration",
                table: "orders",
                columns: new[] { "id", "contact_email", "created_at", "event_id", "organization_id", "status" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "usuarioprueba40@gmail.com", new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), new Guid("11111111-1111-1111-1111-111111111111"), "Confirmed" });

            migrationBuilder.InsertData(
                schema: "registration",
                table: "participants",
                columns: new[] { "id", "full_name", "order_id", "phone", "qr_identifier" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), "Jonathan Rocha", new Guid("22222222-2222-2222-2222-222222222222"), "77712345", "HASH_QR_SEGURO_001" });

            migrationBuilder.InsertData(
                schema: "registration",
                table: "codes",
                columns: new[] { "id", "event_id", "is_used", "organization_id", "token", "used_at", "used_by_participant_id" },
                values: new object[] { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("99999999-9999-9999-9999-999999999999"), true, new Guid("11111111-1111-1111-1111-111111111111"), "DESC-50-VIP", new DateTime(2026, 5, 20, 10, 30, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.CreateIndex(
                name: "ix_codes_token",
                schema: "registration",
                table: "codes",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_codes_used_by_participant_id",
                schema: "registration",
                table: "codes",
                column: "used_by_participant_id");

            migrationBuilder.CreateIndex(
                name: "ix_participants_order_id",
                schema: "registration",
                table: "participants",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_participants_qr_identifier",
                schema: "registration",
                table: "participants",
                column: "qr_identifier",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "codes",
                schema: "registration");

            migrationBuilder.DropTable(
                name: "participants",
                schema: "registration");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "registration");
        }
    }
}
