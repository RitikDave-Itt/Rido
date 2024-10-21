using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rido.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileNAme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Base64String = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

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
                    ProfileImageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Images_ProfileImageId",
                        column: x => x.ProfileImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RideRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RideTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideTransactions", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WalletStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallet_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RazorPayId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RideBookings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideBookings_RideTransactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "RideTransactions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RideBookings_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RideBookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RideReview",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideReview_RideBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "RideBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideReview_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RideReview_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                name: "IX_Images_FileType",
                table: "Images",
                column: "FileType");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideBookings_DriverId",
                table: "RideBookings",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RideBookings_TransactionId",
                table: "RideBookings",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RideBookings_UserId",
                table: "RideBookings",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_RideReview_BookingId",
                table: "RideReview",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RideReview_DriverId",
                table: "RideReview",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RideReview_UserId",
                table: "RideReview",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RideTransactions_DriverId",
                table: "RideTransactions",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RideTransactions_UserId",
                table: "RideTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileImageId",
                table: "Users",
                column: "ProfileImageId",
                unique: true,
                filter: "[ProfileImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_UserId",
                table: "WalletTransactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverData");

            migrationBuilder.DropTable(
                name: "DriverLocations");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RideRequests");

            migrationBuilder.DropTable(
                name: "RideReview");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "RideBookings");

            migrationBuilder.DropTable(
                name: "RideTransactions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
