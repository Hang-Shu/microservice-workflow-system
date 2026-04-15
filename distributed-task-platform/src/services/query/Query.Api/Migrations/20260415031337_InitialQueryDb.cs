using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Query.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialQueryDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "TaskCommentRead",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskCommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserNumber = table.Column<int>(type: "integer", nullable: false),
                    DisplayNameSnapshot = table.Column<string>(type: "text", nullable: false),
                    PreviewText = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCommentRead", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskEmailSummaryRead",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmailId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReciveUserNumber = table.Column<int>(type: "integer", nullable: false),
                    DisplayNameSnapshot = table.Column<string>(type: "text", nullable: false),
                    StatusName = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskEmailSummaryRead", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskModifyLineRead",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskOperatesId = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifyUserNumber = table.Column<int>(type: "integer", nullable: false),
                    DisplayNameSnapshot = table.Column<string>(type: "text", nullable: false),
                    PreviewText = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskModifyLineRead", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskCommentRead",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TaskEmailSummaryRead",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TaskModifyLineRead",
                schema: "public");
        }
    }
}
