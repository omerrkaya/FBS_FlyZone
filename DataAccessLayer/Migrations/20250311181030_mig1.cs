using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    AircraftID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aircraft_Firm = table.Column<int>(type: "int", nullable: false),
                    Aircraft_Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Number_Of_Seats = table.Column<int>(type: "int", nullable: false),
                    Range_KM = table.Column<int>(type: "int", nullable: false),
                    Year_Of_Producation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.AircraftID);
                });

            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    AirlineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Airlines_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AL_IATA_Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    AL_ICAO_Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Central_Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YearOfEstablishment = table.Column<int>(type: "int", nullable: false),
                    Is_It_Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.AirlineID);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    AirportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Airport_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IATA_Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ICAO_Code = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    AP_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AP_Country = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.AirportID);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirlineID = table.Column<int>(type: "int", nullable: false),
                    Departure_Airport = table.Column<int>(type: "int", nullable: false),
                    Arrival_Airport = table.Column<int>(type: "int", nullable: false),
                    Flight_Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Flight_DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estimated_Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    AircraftID = table.Column<int>(type: "int", nullable: false),
                    Flight_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightID);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    PassengerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Passenger_Name_Surname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TcNo_PasaportNo = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Birth_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.PassengerID);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationID = table.Column<int>(type: "int", nullable: false),
                    Payment_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payment_Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Payment_Method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payment_Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightID = table.Column<int>(type: "int", nullable: false),
                    PassengerID = table.Column<int>(type: "int", nullable: false),
                    Seat_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Reservation_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reservation_Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Name_Surname = table.Column<string>(type: "nvarchar(51)", maxLength: 51, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RegisterationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Airlines");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
