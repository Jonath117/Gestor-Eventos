using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payment.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialPaymentSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "payment");

            migrationBuilder.CreateTable(
                name: "transactions",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "manual_receipts",
                schema: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transaction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    file_hash = table.Column<string>(type: "text", nullable: false),
                    mime_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_manual_receipts", x => x.id);
                    table.ForeignKey(
                        name: "fk_manual_receipts_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalSchema: "payment",
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "payment",
                table: "transactions",
                columns: new[] { "id", "amount", "created_at", "order_id", "organization_id", "status" },
                values: new object[] { new Guid("88888888-8888-8888-8888-888888888888"), 150.00m, new DateTime(2026, 5, 20, 1, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111"), "Verified" });

            migrationBuilder.InsertData(
                schema: "payment",
                table: "manual_receipts",
                columns: new[] { "id", "file_hash", "file_url", "mime_type", "transaction_id", "uploaded_at" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "HASH_IMAGEN_SHA256_ABC123", "https://s3.amazonaws.com/tu-bucket/comprobante_001.jpg", "image/jpeg", new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 5, 20, 1, 5, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "ix_manual_receipts_file_hash",
                schema: "payment",
                table: "manual_receipts",
                column: "file_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_manual_receipts_transaction_id",
                schema: "payment",
                table: "manual_receipts",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_order_id",
                schema: "payment",
                table: "transactions",
                column: "order_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "manual_receipts",
                schema: "payment");

            migrationBuilder.DropTable(
                name: "transactions",
                schema: "payment");
        }
    }
}
