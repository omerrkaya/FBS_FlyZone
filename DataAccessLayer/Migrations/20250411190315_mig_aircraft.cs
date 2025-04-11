using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_aircraft : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Aircrafts_AircraftID",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "AircraftID",
                table: "Flights",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Aircrafts_AircraftID",
                table: "Flights",
                column: "AircraftID",
                principalTable: "Aircrafts",
                principalColumn: "AircraftID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Aircrafts_AircraftID",
                table: "Flights");

            migrationBuilder.AlterColumn<int>(
                name: "AircraftID",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Aircrafts_AircraftID",
                table: "Flights",
                column: "AircraftID",
                principalTable: "Aircrafts",
                principalColumn: "AircraftID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
