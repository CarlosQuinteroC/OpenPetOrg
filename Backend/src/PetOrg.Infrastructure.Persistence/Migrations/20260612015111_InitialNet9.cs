using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetOrg.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialNet9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "animal_case_timeline_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AnimalCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ActorId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animal_case_timeline_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "animal_cases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    OpenedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animal_cases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "donation_timeline_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DonationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ActorId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_donation_timeline_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "donations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DonorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Channel = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    ReconciliationStatus = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Reference = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_donations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "donor_consent_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DonorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Granted = table.Column<bool>(type: "boolean", nullable: false),
                    EffectiveAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ActorId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Note = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_donor_consent_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "recurring_donations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DonorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Status = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    StartedOn = table.Column<DateOnly>(type: "date", nullable: false),
                    CancelledOn = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recurring_donations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tax_receipts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DonationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    IssuedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tax_receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tax_receipts_donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_animal_case_timeline_events_AnimalCaseId_At",
                table: "animal_case_timeline_events",
                columns: new[] { "AnimalCaseId", "At" });

            migrationBuilder.CreateIndex(
                name: "IX_donation_timeline_events_DonationId_At",
                table: "donation_timeline_events",
                columns: new[] { "DonationId", "At" });

            migrationBuilder.CreateIndex(
                name: "IX_tax_receipts_DonationId",
                table: "tax_receipts",
                column: "DonationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "animal_case_timeline_events");

            migrationBuilder.DropTable(
                name: "animal_cases");

            migrationBuilder.DropTable(
                name: "donation_timeline_events");

            migrationBuilder.DropTable(
                name: "donor_consent_events");

            migrationBuilder.DropTable(
                name: "recurring_donations");

            migrationBuilder.DropTable(
                name: "tax_receipts");

            migrationBuilder.DropTable(
                name: "donations");
        }
    }
}
