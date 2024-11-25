using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RezervacijaKarata.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPromoCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingDayZones_Bookings_BookingId",
                table: "BookingDayZones");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingDayZones_RaceDays_RaceDayId",
                table: "BookingDayZones");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingDayZones_SeatingZones_SeatingZoneId",
                table: "BookingDayZones");

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedByBookingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromoCodes_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_BookingId",
                table: "PromoCodes",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDayZones_Bookings_BookingId",
                table: "BookingDayZones",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDayZones_RaceDays_RaceDayId",
                table: "BookingDayZones",
                column: "RaceDayId",
                principalTable: "RaceDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDayZones_SeatingZones_SeatingZoneId",
                table: "BookingDayZones",
                column: "SeatingZoneId",
                principalTable: "SeatingZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingDayZones_Bookings_BookingId",
                table: "BookingDayZones");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingDayZones_RaceDays_RaceDayId",
                table: "BookingDayZones");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingDayZones_SeatingZones_SeatingZoneId",
                table: "BookingDayZones");

            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDayZones_Bookings_BookingId",
                table: "BookingDayZones",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDayZones_RaceDays_RaceDayId",
                table: "BookingDayZones",
                column: "RaceDayId",
                principalTable: "RaceDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDayZones_SeatingZones_SeatingZoneId",
                table: "BookingDayZones",
                column: "SeatingZoneId",
                principalTable: "SeatingZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
