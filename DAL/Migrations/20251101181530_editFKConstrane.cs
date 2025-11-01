using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class editFKConstrane : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airplanes_Airlines_AirlineId",
                table: "Airplanes");

            migrationBuilder.AddForeignKey(
                name: "FK_Airplanes_Airlines_AirlineId",
                table: "Airplanes",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airplanes_Airlines_AirlineId",
                table: "Airplanes");

            migrationBuilder.AddForeignKey(
                name: "FK_Airplanes_Airlines_AirlineId",
                table: "Airplanes",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
