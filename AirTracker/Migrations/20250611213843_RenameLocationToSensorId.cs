using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTracker.Migrations
{
    /// <inheritdoc />
    public partial class RenameLocationToSensorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenAQLocationId",
                table: "Sensors",
                newName: "OpenAQSensorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenAQSensorId",
                table: "Sensors",
                newName: "OpenAQLocationId");
        }
    }
}
