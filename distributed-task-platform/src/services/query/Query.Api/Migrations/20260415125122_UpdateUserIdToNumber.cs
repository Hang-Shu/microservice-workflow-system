using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Query.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserIdToNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskId",
                schema: "public",
                table: "TaskModifyLineRead");

            migrationBuilder.DropColumn(
                name: "TaskId",
                schema: "public",
                table: "TaskEmailSummaryRead");

            migrationBuilder.DropColumn(
                name: "TaskId",
                schema: "public",
                table: "TaskCommentRead");

            migrationBuilder.AddColumn<int>(
                name: "TaskNumber",
                schema: "public",
                table: "TaskModifyLineRead",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskNumber",
                schema: "public",
                table: "TaskEmailSummaryRead",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskNumber",
                schema: "public",
                table: "TaskCommentRead",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskNumber",
                schema: "public",
                table: "TaskModifyLineRead");

            migrationBuilder.DropColumn(
                name: "TaskNumber",
                schema: "public",
                table: "TaskEmailSummaryRead");

            migrationBuilder.DropColumn(
                name: "TaskNumber",
                schema: "public",
                table: "TaskCommentRead");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                schema: "public",
                table: "TaskModifyLineRead",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                schema: "public",
                table: "TaskEmailSummaryRead",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                schema: "public",
                table: "TaskCommentRead",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
