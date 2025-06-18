using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationAndLocationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Sensors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_LocationId",
                table: "Sensors",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_LocationId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Sensors");
        }
    }
}
