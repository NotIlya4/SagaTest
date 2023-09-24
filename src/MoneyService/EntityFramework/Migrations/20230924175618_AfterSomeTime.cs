using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyService.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AfterSomeTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Idempotencies");

            migrationBuilder.CreateTable(
                name: "IdempotencyToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Response = table.Column<string>(type: "text", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdempotencyToken", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdempotencyToken");

            migrationBuilder.CreateTable(
                name: "Idempotencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idempotencies", x => x.Id);
                });
        }
    }
}
