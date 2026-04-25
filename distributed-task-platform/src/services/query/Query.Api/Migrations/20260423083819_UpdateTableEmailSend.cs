using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Query.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableEmailSend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdempotentId",
                schema: "public",
                table: "TaskEmailSummaryRead",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEmailSummaryRead_IdempotentId",
                schema: "public",
                table: "TaskEmailSummaryRead",
                column: "IdempotentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskEmailSummaryRead_IdempotentId",
                schema: "public",
                table: "TaskEmailSummaryRead");

            migrationBuilder.DropColumn(
                name: "IdempotentId",
                schema: "public",
                table: "TaskEmailSummaryRead");
        }
    }
}
