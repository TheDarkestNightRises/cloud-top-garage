using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnvironmentService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Garages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndoorEnvironmentSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Co2Limit = table.Column<int>(type: "int", nullable: false),
                    TemperatureLimit = table.Column<float>(type: "real", nullable: false),
                    HumidityLimit = table.Column<float>(type: "real", nullable: false),
                    LightLimit = table.Column<float>(type: "real", nullable: false),
                    LightOn = table.Column<bool>(type: "bit", nullable: false),
                    MacAddress = table.Column<int>(type: "int", nullable: false),
                    LoRaWANURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Eui = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndoorEnvironmentSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndoorEnvironments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GarageId = table.Column<int>(type: "int", nullable: false),
                    IndoorEnvironmentSettingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndoorEnvironments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndoorEnvironments_Garages_GarageId",
                        column: x => x.GarageId,
                        principalTable: "Garages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndoorEnvironments_IndoorEnvironmentSettings_IndoorEnvironmentSettingsId",
                        column: x => x.IndoorEnvironmentSettingsId,
                        principalTable: "IndoorEnvironmentSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndoorEnvironmentId = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    Humidity = table.Column<float>(type: "real", nullable: false),
                    CO2 = table.Column<float>(type: "real", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stats_IndoorEnvironments_IndoorEnvironmentId",
                        column: x => x.IndoorEnvironmentId,
                        principalTable: "IndoorEnvironments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndoorEnvironments_GarageId",
                table: "IndoorEnvironments",
                column: "GarageId");

            migrationBuilder.CreateIndex(
                name: "IX_IndoorEnvironments_IndoorEnvironmentSettingsId",
                table: "IndoorEnvironments",
                column: "IndoorEnvironmentSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Stats_IndoorEnvironmentId",
                table: "Stats",
                column: "IndoorEnvironmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stats");

            migrationBuilder.DropTable(
                name: "IndoorEnvironments");

            migrationBuilder.DropTable(
                name: "Garages");

            migrationBuilder.DropTable(
                name: "IndoorEnvironmentSettings");
        }
    }
}
