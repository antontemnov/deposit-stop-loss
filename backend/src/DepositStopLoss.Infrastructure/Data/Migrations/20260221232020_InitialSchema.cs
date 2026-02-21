using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace DepositStopLoss.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "banks",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deposit_snapshots",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    deposit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exchange_rate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    profit_loss_percent = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    snapshot_date = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    accrued_interest = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    accrued_interest_currency = table.Column<int>(type: "integer", nullable: false),
                    total_amount_deposit_currency = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    total_amount_deposit_currency_code = table.Column<int>(type: "integer", nullable: false),
                    total_amount_usd = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    total_amount_usd_currency = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_snapshots", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deposits",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    annual_interest_rate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    bank_id = table.Column<Guid>(type: "uuid", nullable: false),
                    closed_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    currency = table.Column<int>(type: "integer", nullable: false),
                    maturity_date = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    opened_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    rate_type = table.Column<int>(type: "integer", nullable: false),
                    source = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    stop_loss_threshold = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    term_months = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exchange_rates",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_id = table.Column<Guid>(type: "uuid", nullable: false),
                    buy_rate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    fetched_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    from_currency = table.Column<int>(type: "integer", nullable: false),
                    rate_date = table.Column<LocalDate>(type: "date", nullable: false),
                    rate_type = table.Column<int>(type: "integer", nullable: false),
                    sell_rate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    to_currency = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exchange_rates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    deposit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    error_message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    level = table.Column<int>(type: "integer", nullable: false),
                    message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    read_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    sent_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_bank_connections",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    access_token = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    bank_id = table.Column<Guid>(type: "uuid", nullable: false),
                    connected_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    last_sync_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    refresh_token = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    scopes = table.Column<string[]>(type: "text[]", nullable: false),
                    token_expires_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_bank_connections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    enable_email_notifications = table.Column<bool>(type: "boolean", nullable: false),
                    enable_telegram_notifications = table.Column<bool>(type: "boolean", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    last_activity_at = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    registered_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    telegram_id = table.Column<long>(type: "bigint", nullable: false),
                    timezone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bank_rate_sources",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    api_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    bank_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rate_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_rate_sources", x => x.id);
                    table.ForeignKey(
                        name: "FK_bank_rate_sources_banks_bank_id",
                        column: x => x.bank_id,
                        principalSchema: "public",
                        principalTable: "banks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deposit_contributions",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    contributed_at = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    deposit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exchange_rate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deposit_contributions", x => x.id);
                    table.ForeignKey(
                        name: "FK_deposit_contributions_deposits_deposit_id",
                        column: x => x.deposit_id,
                        principalSchema: "public",
                        principalTable: "deposits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bank_rate_sources_bank_rate_type",
                schema: "public",
                table: "bank_rate_sources",
                columns: new[] { "bank_id", "rate_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_banks_code",
                schema: "public",
                table: "banks",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deposit_contributions_deposit_id",
                schema: "public",
                table: "deposit_contributions",
                column: "deposit_id");

            migrationBuilder.CreateIndex(
                name: "ix_deposit_snapshots_deposit_date",
                schema: "public",
                table: "deposit_snapshots",
                columns: new[] { "deposit_id", "snapshot_date" });

            migrationBuilder.CreateIndex(
                name: "ix_deposit_snapshots_deposit_id",
                schema: "public",
                table: "deposit_snapshots",
                column: "deposit_id");

            migrationBuilder.CreateIndex(
                name: "ix_deposits_bank_id",
                schema: "public",
                table: "deposits",
                column: "bank_id");

            migrationBuilder.CreateIndex(
                name: "ix_deposits_status",
                schema: "public",
                table: "deposits",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_deposits_user_id",
                schema: "public",
                table: "deposits",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_exchange_rates_bank_currencies_date_type",
                schema: "public",
                table: "exchange_rates",
                columns: new[] { "bank_id", "from_currency", "to_currency", "rate_date", "rate_type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_exchange_rates_rate_date",
                schema: "public",
                table: "exchange_rates",
                column: "rate_date");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_status",
                schema: "public",
                table: "notifications",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id",
                schema: "public",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_status",
                schema: "public",
                table: "notifications",
                columns: new[] { "user_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_user_bank_connections_user_bank",
                schema: "public",
                table: "user_bank_connections",
                columns: new[] { "user_id", "bank_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_telegram_id",
                schema: "public",
                table: "users",
                column: "telegram_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bank_rate_sources",
                schema: "public");

            migrationBuilder.DropTable(
                name: "deposit_contributions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "deposit_snapshots",
                schema: "public");

            migrationBuilder.DropTable(
                name: "exchange_rates",
                schema: "public");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "public");

            migrationBuilder.DropTable(
                name: "user_bank_connections",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "banks",
                schema: "public");

            migrationBuilder.DropTable(
                name: "deposits",
                schema: "public");
        }
    }
}
