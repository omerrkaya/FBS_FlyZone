using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_Passanger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Passengers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_UserID",
                table: "Passengers",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Users_UserID",
                table: "Passengers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Users_UserID",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_UserID",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Passengers");
        }
    }
}
