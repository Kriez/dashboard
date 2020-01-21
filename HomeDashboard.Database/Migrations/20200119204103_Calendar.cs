using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace kriez.HomeDashboard.Data.Migrations
{
    public partial class Calendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HueLights_HueScenes_HueSceneId",
                table: "HueLights");

            migrationBuilder.DropIndex(
                name: "IX_HueLights_HueSceneId",
                table: "HueLights");

            migrationBuilder.DropColumn(
                name: "HueSceneId",
                table: "HueLights");

            migrationBuilder.CreateTable(
                name: "CalendarItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: true),
                    End = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarItems");

            migrationBuilder.AddColumn<string>(
                name: "HueSceneId",
                table: "HueLights",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HueLights_HueSceneId",
                table: "HueLights",
                column: "HueSceneId");

            migrationBuilder.AddForeignKey(
                name: "FK_HueLights_HueScenes_HueSceneId",
                table: "HueLights",
                column: "HueSceneId",
                principalTable: "HueScenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
