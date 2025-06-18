using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddOpenAQLocationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Sensors");

            migrationBuilder.AddColumn<int>(
                name: "OpenAQLocationId",
                table: "Sensors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenAQLocationId",
                table: "Sensors");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Sensors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Sensors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
