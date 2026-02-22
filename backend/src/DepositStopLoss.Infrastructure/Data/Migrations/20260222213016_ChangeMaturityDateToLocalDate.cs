using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace DepositStopLoss.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMaturityDateToLocalDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalDate>(
                name: "maturity_date",
                schema: "public",
                table: "deposits",
                type: "date",
                nullable: false,
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Instant>(
                name: "maturity_date",
                schema: "public",
                table: "deposits",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(LocalDate),
                oldType: "date");
        }
    }
}
