using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rido.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverLocations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Geohash = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLocations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverLocations_DriverId",
                table: "DriverLocations",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverLocations_Geohash",
                table: "DriverLocations",
                column: "Geohash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverLocations");
        }
    }
}
