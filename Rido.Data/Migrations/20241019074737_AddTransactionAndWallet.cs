using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rido.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionAndWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneTimePasswords");

            migrationBuilder.CreateTable(
                name: "RideBookings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PickupTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DropoffTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickupLatitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupLongitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationLatitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationLongitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    DistanceInKm = table.Column<double>(type: "float", nullable: false),
                    GeohashCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideBookings_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RideBookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RideTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideTransactions_RideBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "RideBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideTransactions_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RideTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideBookings_DriverId",
                table: "RideBookings",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RideBookings_TransactionId",
                table: "RideBookings",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideBookings_UserId",
                table: "RideBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RideTransactions_BookingId",
                table: "RideTransactions",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_RideTransactions_DriverId",
                table: "RideTransactions",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RideTransactions_UserId",
                table: "RideTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideBookings_RideTransactions_TransactionId",
                table: "RideBookings",
                column: "TransactionId",
                principalTable: "RideTransactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideBookings_RideTransactions_TransactionId",
                table: "RideBookings");

            migrationBuilder.DropTable(
                name: "RideTransactions");

            migrationBuilder.DropTable(
                name: "RideBookings");

            migrationBuilder.CreateTable(
                name: "OneTimePasswords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RideRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    TryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneTimePasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneTimePasswords_RideRequests_RideRequestId",
                        column: x => x.RideRequestId,
                        principalTable: "RideRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_RideRequestId",
                table: "OneTimePasswords",
                column: "RideRequestId",
                unique: true);
        }
    }
}
