using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Query.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableTaskComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPreview",
                schema: "public",
                table: "TaskCommentRead",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                schema: "public",
                table: "TaskCommentRead",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPreview",
                schema: "public",
                table: "TaskCommentRead");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                schema: "public",
                table: "TaskCommentRead");
        }
    }
}
