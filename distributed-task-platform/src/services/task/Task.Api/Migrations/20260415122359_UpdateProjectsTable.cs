using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                schema: "public",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserNumber",
                schema: "public",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsVaild",
                schema: "public",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUserNumber",
                schema: "public",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsVaild",
                schema: "public",
                table: "Projects");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                schema: "public",
                table: "Projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
