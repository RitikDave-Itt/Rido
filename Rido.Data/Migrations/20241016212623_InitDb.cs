using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rido.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DriverData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LicenseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    VehicleRegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VehicleModel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VehicleMake = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverLocations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Geohash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverLocations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RideRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PickupLatitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupLongitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickupTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DestinationLatitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationLongitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    GeohashCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DistanceInKm = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideRequests_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RideRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverData_UserId",
                table: "DriverData",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverLocations_Geohash",
                table: "DriverLocations",
                column: "Geohash");

            migrationBuilder.CreateIndex(
                name: "IX_DriverLocations_UserId",
                table: "DriverLocations",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideRequests_DriverId",
                table: "RideRequests",
                column: "DriverId",
                unique: true,
                filter: "[DriverId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RideRequests_GeohashCode",
                table: "RideRequests",
                column: "GeohashCode");

            migrationBuilder.CreateIndex(
                name: "IX_RideRequests_UserId",
                table: "RideRequests",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideRequests_VehicleType",
                table: "RideRequests",
                column: "VehicleType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverData");

            migrationBuilder.DropTable(
                name: "DriverLocations");

            migrationBuilder.DropTable(
                name: "RideRequests");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
