using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rido.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOTPTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneTimePasswords",
                columns: table => new
                {
                    RideRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    TryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneTimePasswords", x => x.RideRequestId);
                    table.ForeignKey(
                        name: "FK_OneTimePasswords_RideRequests_RideRequestId",
                        column: x => x.RideRequestId,
                        principalTable: "RideRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneTimePasswords");
        }
    }
}
