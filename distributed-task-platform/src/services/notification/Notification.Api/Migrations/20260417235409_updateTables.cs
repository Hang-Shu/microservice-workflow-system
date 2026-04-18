using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "readTime",
                schema: "public",
                table: "Notifications_Dtl",
                newName: "ReadTime");

            migrationBuilder.RenameColumn(
                name: "MsgTime",
                schema: "public",
                table: "Notifications",
                newName: "CreateTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadTime",
                schema: "public",
                table: "Notifications_Dtl",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "SendTime",
                schema: "public",
                table: "Notifications_Dtl",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NotificationCategory",
                schema: "public",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NotificationInbox",
                schema: "public",
                columns: table => new
                {
                    EventKey = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationInbox", x => x.EventKey);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationInbox",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "SendTime",
                schema: "public",
                table: "Notifications_Dtl");

            migrationBuilder.DropColumn(
                name: "NotificationCategory",
                schema: "public",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "ReadTime",
                schema: "public",
                table: "Notifications_Dtl",
                newName: "readTime");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                schema: "public",
                table: "Notifications",
                newName: "MsgTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "readTime",
                schema: "public",
                table: "Notifications_Dtl",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
