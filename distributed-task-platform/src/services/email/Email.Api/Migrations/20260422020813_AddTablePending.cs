using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Email.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePending : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSent",
                schema: "public",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "ReciveUserId",
                schema: "public",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "TaskId",
                schema: "public",
                table: "Emails");

            migrationBuilder.RenameColumn(
                name: "ErrorMsg",
                schema: "public",
                table: "Emails",
                newName: "LastErrorMsg");

            migrationBuilder.AddColumn<int>(
                name: "EmailStatus",
                schema: "public",
                table: "Emails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReciveUserNumber",
                schema: "public",
                table: "Emails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TasksEmailsPending",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReciveUserNumber = table.Column<int>(type: "integer", nullable: false),
                    EmailAccount = table.Column<string>(type: "text", nullable: false),
                    PendingTaskNumbers = table.Column<List<int>>(type: "integer[]", nullable: false),
                    TaskTitles = table.Column<string>(type: "text", nullable: false),
                    FirstEventTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlanSentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksEmailsPending", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TasksEmailsPending",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "EmailStatus",
                schema: "public",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "ReciveUserNumber",
                schema: "public",
                table: "Emails");

            migrationBuilder.RenameColumn(
                name: "LastErrorMsg",
                schema: "public",
                table: "Emails",
                newName: "ErrorMsg");

            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                schema: "public",
                table: "Emails",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ReciveUserId",
                schema: "public",
                table: "Emails",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                schema: "public",
                table: "Emails",
                type: "uuid",
                nullable: true);
        }
    }
}
