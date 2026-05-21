using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

            // migrationBuilder.CreateTable(
            //     name: "users",
            //     schema: "core",
            //     columns: table => new
            //     {
            //         id = table.Column<Guid>(type: "uuid", nullable: false),
            //         email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            //         password_hash = table.Column<string>(type: "text", nullable: false),
            //         created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("pk_users", x => x.id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "organization_users",
            //     schema: "core",
            //     columns: table => new
            //     {
            //         organization_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         user_id = table.Column<Guid>(type: "uuid", nullable: false),
            //         role = table.Column<string>(type: "text", nullable: false),
            //         joined_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("pk_organization_users", x => new { x.organization_id, x.user_id });
            //         table.ForeignKey(
            //             name: "fk_organization_users_users_user_id",
            //             column: x => x.user_id,
            //             principalSchema: "core",
            //             principalTable: "users",
            //             principalColumn: "id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            migrationBuilder.CreateTable(
                name: "user_refresh_tokens",
                schema: "core",
                columns: table => new
                {
                    token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    replaced_by_token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_refresh_tokens", x => x.token);
                    table.ForeignKey(
                        name: "fk_user_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "core",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            // migrationBuilder.CreateIndex(
            //     name: "ix_organization_users_user_id",
            //     schema: "core",
            //     table: "organization_users",
            //     column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_refresh_tokens_user_id",
                schema: "core",
                table: "user_refresh_tokens",
                column: "user_id");

            // migrationBuilder.CreateIndex(
            //     name: "ix_users_email",
            //     schema: "core",
            //     table: "users",
            //     column: "email",
            //     unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organization_users",
                schema: "core");

            migrationBuilder.DropTable(
                name: "user_refresh_tokens",
                schema: "core");

            migrationBuilder.DropTable(
                name: "users",
                schema: "core");
        }
    }
}
