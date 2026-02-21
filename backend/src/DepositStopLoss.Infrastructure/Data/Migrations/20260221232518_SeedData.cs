using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DepositStopLoss.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "banks",
                columns: new[] { "id", "code", "is_active", "name", "type" },
                values: new object[,]
                {
                    { new Guid("39364dee-7cb1-4273-8dfc-6efa5f5c1c53"), "TBC", true, "TBC Bank", 1 },
                    { new Guid("afbb4296-a5f8-428f-a7c6-9edc9c7b5673"), "BOG", true, "Bank of Georgia", 1 },
                    { new Guid("c73e6bfc-5efe-41d1-b80a-8cf425e75b3b"), "NBG", true, "National Bank of Georgia", 2 }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "bank_rate_sources",
                columns: new[] { "id", "api_url", "bank_id", "rate_type" },
                values: new object[,]
                {
                    { new Guid("6099cfd8-8a37-4b84-85b3-b0b880394cac"), "https://api.tbcbank.ge/v1/exchange-rates/concept", new Guid("39364dee-7cb1-4273-8dfc-6efa5f5c1c53"), 2 },
                    { new Guid("ba1a049a-7bb2-4213-835f-56b20aa13dfc"), "https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/en/json", new Guid("c73e6bfc-5efe-41d1-b80a-8cf425e75b3b"), 1 },
                    { new Guid("ccba9c58-034a-4b1d-8431-c74249969c0f"), "https://api.tbcbank.ge/v1/exchange-rates/commercial", new Guid("39364dee-7cb1-4273-8dfc-6efa5f5c1c53"), 1 },
                    { new Guid("eaab9b5a-86e2-47dc-adad-9330cf6d9263"), "https://bankofgeorgia.ge/api/currencies", new Guid("afbb4296-a5f8-428f-a7c6-9edc9c7b5673"), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "bank_rate_sources",
                keyColumn: "id",
                keyValue: new Guid("6099cfd8-8a37-4b84-85b3-b0b880394cac"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "bank_rate_sources",
                keyColumn: "id",
                keyValue: new Guid("ba1a049a-7bb2-4213-835f-56b20aa13dfc"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "bank_rate_sources",
                keyColumn: "id",
                keyValue: new Guid("ccba9c58-034a-4b1d-8431-c74249969c0f"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "bank_rate_sources",
                keyColumn: "id",
                keyValue: new Guid("eaab9b5a-86e2-47dc-adad-9330cf6d9263"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "banks",
                keyColumn: "id",
                keyValue: new Guid("39364dee-7cb1-4273-8dfc-6efa5f5c1c53"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "banks",
                keyColumn: "id",
                keyValue: new Guid("afbb4296-a5f8-428f-a7c6-9edc9c7b5673"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "banks",
                keyColumn: "id",
                keyValue: new Guid("c73e6bfc-5efe-41d1-b80a-8cf425e75b3b"));
        }
    }
}
