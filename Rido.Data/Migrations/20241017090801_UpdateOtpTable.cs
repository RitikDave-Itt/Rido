using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rido.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOtpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OneTimePasswords",
                table: "OneTimePasswords");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "OneTimePasswords",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OneTimePasswords",
                table: "OneTimePasswords",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePasswords_RideRequestId",
                table: "OneTimePasswords",
                column: "RideRequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OneTimePasswords",
                table: "OneTimePasswords");

            migrationBuilder.DropIndex(
                name: "IX_OneTimePasswords_RideRequestId",
                table: "OneTimePasswords");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OneTimePasswords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OneTimePasswords",
                table: "OneTimePasswords",
                column: "RideRequestId");
        }
    }
}
