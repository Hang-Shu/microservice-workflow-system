using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                schema: "public",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserNumber",
                schema: "public",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    ProjectDescribe = table.Column<string>(type: "text", nullable: true),
                    CreateUserNumber = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsVaild = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TaskComment",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommentText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    UderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskOperates",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    OperateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskOperates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskOperates_Dtl",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MainId = table.Column<Guid>(type: "uuid", nullable: false),
                    OpreateType = table.Column<string>(type: "text", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: false),
                    NewValue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskOperates_Dtl", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TaskComment",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TaskOperates",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TaskOperates_Dtl",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "CreatedUserNumber",
                schema: "public",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                schema: "public",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
