using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarageService.Migrations
{
    /// <inheritdoc />
    public partial class GarageEntityUpdatedCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Capacity",
                table: "Garages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Garages");
        }
    }
}
