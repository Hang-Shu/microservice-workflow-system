using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Query.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateTableTimeLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviewText",
                schema: "public",
                table: "TaskModifyLineRead");

            migrationBuilder.DropColumn(
                name: "TaskOperatesId",
                schema: "public",
                table: "TaskModifyLineRead");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                schema: "public",
                table: "TaskModifyLineRead",
                newName: "GroupStartTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "GroupEndTime",
                schema: "public",
                table: "TaskModifyLineRead",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<List<string>>(
                name: "ListUpdateItems",
                schema: "public",
                table: "TaskModifyLineRead",
                type: "text[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupEndTime",
                schema: "public",
                table: "TaskModifyLineRead");

            migrationBuilder.DropColumn(
                name: "ListUpdateItems",
                schema: "public",
                table: "TaskModifyLineRead");

            migrationBuilder.RenameColumn(
                name: "GroupStartTime",
                schema: "public",
                table: "TaskModifyLineRead",
                newName: "CreateTime");

            migrationBuilder.AddColumn<string>(
                name: "PreviewText",
                schema: "public",
                table: "TaskModifyLineRead",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskOperatesId",
                schema: "public",
                table: "TaskModifyLineRead",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
