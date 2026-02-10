using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspireTickets.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class Seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TicketItems",
                keyColumn: "Id",
                keyValue: new Guid("0c30022b-8617-47ec-8e2d-6f327f507084"));

            migrationBuilder.DeleteData(
                table: "TicketItems",
                keyColumn: "Id",
                keyValue: new Guid("e455f48f-35d1-4fa4-aaf1-4f7fcf5da22a"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TicketItems",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Subject" },
                values: new object[,]
                {
                    { new Guid("0c30022b-8617-47ec-8e2d-6f327f507084"), "Sam Sample", new DateTime(2026, 2, 3, 23, 10, 1, 772, DateTimeKind.Utc).AddTicks(8664), "Jane Smith's account has issues.", "Account issue" },
                    { new Guid("e455f48f-35d1-4fa4-aaf1-4f7fcf5da22a"), "Sam Sample", new DateTime(2026, 2, 3, 23, 10, 1, 772, DateTimeKind.Utc).AddTicks(8216), "I experienced an issue with my recent order.", "Problem with order #1234" }
                });
        }
    }
}
