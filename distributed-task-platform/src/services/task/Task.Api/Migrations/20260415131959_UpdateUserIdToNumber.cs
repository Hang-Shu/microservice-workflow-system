using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserIdToNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                schema: "public",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                schema: "public",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskId",
                schema: "public",
                table: "TaskOperates");

            migrationBuilder.DropColumn(
                name: "UserID",
                schema: "public",
                table: "TaskOperates");

            migrationBuilder.DropColumn(
                name: "TaskId",
                schema: "public",
                table: "TaskComment");

            migrationBuilder.DropColumn(
                name: "UderId",
                schema: "public",
                table: "TaskComment");

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserNumber",
                schema: "public",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserNumber",
                schema: "public",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskNumber",
                schema: "public",
                table: "TaskOperates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserNumber",
                schema: "public",
                table: "TaskOperates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskNumber",
                schema: "public",
                table: "TaskComment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserNumber",
                schema: "public",
                table: "TaskComment",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUserNumber",
                schema: "public",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedUserNumber",
                schema: "public",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskNumber",
                schema: "public",
                table: "TaskOperates");

            migrationBuilder.DropColumn(
                name: "UserNumber",
                schema: "public",
                table: "TaskOperates");

            migrationBuilder.DropColumn(
                name: "TaskNumber",
                schema: "public",
                table: "TaskComment");

            migrationBuilder.DropColumn(
                name: "UserNumber",
                schema: "public",
                table: "TaskComment");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedUserId",
                schema: "public",
                table: "Tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                schema: "public",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                schema: "public",
                table: "TaskOperates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                schema: "public",
                table: "TaskOperates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                schema: "public",
                table: "TaskComment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UderId",
                schema: "public",
                table: "TaskComment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
