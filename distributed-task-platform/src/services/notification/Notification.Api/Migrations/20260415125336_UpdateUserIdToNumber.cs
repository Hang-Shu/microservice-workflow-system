using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserIdToNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReciveUserId",
                schema: "public",
                table: "Notifications_Dtl");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                schema: "public",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "ReciveUserNumber",
                schema: "public",
                table: "Notifications_Dtl",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FromUserNumber",
                schema: "public",
                table: "Notifications",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReciveUserNumber",
                schema: "public",
                table: "Notifications_Dtl");

            migrationBuilder.DropColumn(
                name: "FromUserNumber",
                schema: "public",
                table: "Notifications");

            migrationBuilder.AddColumn<Guid>(
                name: "ReciveUserId",
                schema: "public",
                table: "Notifications_Dtl",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FromUserId",
                schema: "public",
                table: "Notifications",
                type: "uuid",
                nullable: true);
        }
    }
}
