using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyService.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Migr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdempotencyToken",
                table: "IdempotencyToken");

            migrationBuilder.RenameTable(
                name: "IdempotencyToken",
                newName: "IdempotencyTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdempotencyTokens",
                table: "IdempotencyTokens",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdempotencyTokens",
                table: "IdempotencyTokens");

            migrationBuilder.RenameTable(
                name: "IdempotencyTokens",
                newName: "IdempotencyToken");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdempotencyToken",
                table: "IdempotencyToken",
                column: "Id");
        }
    }
}
