using Microsoft.EntityFrameworkCore.Migrations;

namespace kriez.HomeDashboard.Data.Migrations
{
    public partial class BrightnessAndColorToLight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Brightness",
                table: "HueLights",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "HueLights",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brightness",
                table: "HueLights");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "HueLights");
        }
    }
}
