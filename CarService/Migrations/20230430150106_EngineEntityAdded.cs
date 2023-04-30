using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarService.Migrations
{
    /// <inheritdoc />
    public partial class EngineEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "EngineId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Seats",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Engine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<double>(type: "float", nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PowerHP = table.Column<int>(type: "int", nullable: false),
                    TorqueNM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engine", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_EngineId",
                table: "Cars",
                column: "EngineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Engine_EngineId",
                table: "Cars",
                column: "EngineId",
                principalTable: "Engine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Engine_EngineId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Engine");

            migrationBuilder.DropIndex(
                name: "IX_Cars_EngineId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "EngineId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Seats",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Cars");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
