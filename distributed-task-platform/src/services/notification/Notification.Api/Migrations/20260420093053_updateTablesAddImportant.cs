using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateTablesAddImportant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImportant",
                schema: "public",
                table: "Notifications_Dtl",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImportant",
                schema: "public",
                table: "Notifications_Dtl");
        }
    }
}
