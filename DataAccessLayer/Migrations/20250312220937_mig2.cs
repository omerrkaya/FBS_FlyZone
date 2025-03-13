using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArrivalAirportAirportID",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartureAirportAirportID",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationID",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_FlightID",
                table: "Reservations",
                column: "FlightID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PassengerID",
                table: "Reservations",
                column: "PassengerID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReservationID",
                table: "Payments",
                column: "ReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftID",
                table: "Flights",
                column: "AircraftID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineID",
                table: "Flights",
                column: "AirlineID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ArrivalAirportAirportID",
                table: "Flights",
                column: "ArrivalAirportAirportID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DepartureAirportAirportID",
                table: "Flights",
                column: "DepartureAirportAirportID");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ReservationID",
                table: "Flights",
                column: "ReservationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Aircrafts_AircraftID",
                table: "Flights",
                column: "AircraftID",
                principalTable: "Aircrafts",
                principalColumn: "AircraftID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airlines_AirlineID",
                table: "Flights",
                column: "AirlineID",
                principalTable: "Airlines",
                principalColumn: "AirlineID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_ArrivalAirportAirportID",
                table: "Flights",
                column: "ArrivalAirportAirportID",
                principalTable: "Airports",
                principalColumn: "AirportID");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Airports_DepartureAirportAirportID",
                table: "Flights",
                column: "DepartureAirportAirportID",
                principalTable: "Airports",
                principalColumn: "AirportID");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Reservations_ReservationID",
                table: "Flights",
                column: "ReservationID",
                principalTable: "Reservations",
                principalColumn: "ReservationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Reservations_ReservationID",
                table: "Payments",
                column: "ReservationID",
                principalTable: "Reservations",
                principalColumn: "ReservationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Flights_FlightID",
                table: "Reservations",
                column: "FlightID",
                principalTable: "Flights",
                principalColumn: "FlightID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Passengers_PassengerID",
                table: "Reservations",
                column: "PassengerID",
                principalTable: "Passengers",
                principalColumn: "PassengerID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Aircrafts_AircraftID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airlines_AirlineID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_ArrivalAirportAirportID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Airports_DepartureAirportAirportID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Reservations_ReservationID",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Reservations_ReservationID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Flights_FlightID",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Passengers_PassengerID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_FlightID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_PassengerID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ReservationID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Flights_AircraftID",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_AirlineID",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_ArrivalAirportAirportID",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_DepartureAirportAirportID",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_ReservationID",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ArrivalAirportAirportID",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DepartureAirportAirportID",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ReservationID",
                table: "Flights");
        }
    }
}
