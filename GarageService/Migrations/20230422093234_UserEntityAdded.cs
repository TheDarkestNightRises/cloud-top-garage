using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageService.Migrations
{
    /// <inheritdoc />
    public partial class UserEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Garages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Garages_OwnerId",
                table: "Garages",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Garages_Users_OwnerId",
                table: "Garages",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Garages_Users_OwnerId",
                table: "Garages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Garages_OwnerId",
                table: "Garages");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Garages");
        }
    }
}
