using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirQualities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirQualities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorId = table.Column<int>(type: "int", nullable: false),
                    NO2 = table.Column<double>(type: "float", nullable: false),
                    PM10 = table.Column<double>(type: "float", nullable: false),
                    PM25 = table.Column<double>(type: "float", nullable: false),
                    RetrievedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirQualities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirQualities_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirQualities_SensorId",
                table: "AirQualities",
                column: "SensorId");
        }
    }
}
