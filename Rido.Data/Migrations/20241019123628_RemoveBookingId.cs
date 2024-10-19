using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rido.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBookingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideTransactions_RideBookings_BookingId",
                table: "RideTransactions");

            migrationBuilder.DropIndex(
                name: "IX_RideTransactions_BookingId",
                table: "RideTransactions");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "RideTransactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingId",
                table: "RideTransactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RideTransactions_BookingId",
                table: "RideTransactions",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideTransactions_RideBookings_BookingId",
                table: "RideTransactions",
                column: "BookingId",
                principalTable: "RideBookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
