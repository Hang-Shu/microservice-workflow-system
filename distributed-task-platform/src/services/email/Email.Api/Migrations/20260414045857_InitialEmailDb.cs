using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Email.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialEmailDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Emails",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReciveUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmailAccount = table.Column<string>(type: "text", nullable: false),
                    EmailTitle = table.Column<string>(type: "text", nullable: false),
                    EmailText = table.Column<string>(type: "text", nullable: false),
                    IsSent = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ErrorMsg = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emails",
                schema: "public");
        }
    }
}
