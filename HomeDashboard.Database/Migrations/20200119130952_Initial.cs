using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace kriez.HomeDashboard.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HueScenes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HueScenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpdateTables",
                columns: table => new
                {
                    Key = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateTables", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "HueLights",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsReachable = table.Column<bool>(nullable: true),
                    IsOn = table.Column<bool>(nullable: false),
                    Group = table.Column<string>(nullable: true),
                    HueSceneId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HueLights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HueLights_HueScenes_Group",
                        column: x => x.Group,
                        principalTable: "HueScenes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HueLights_HueScenes_HueSceneId",
                        column: x => x.HueSceneId,
                        principalTable: "HueScenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HueLights_Group",
                table: "HueLights",
                column: "Group");

            migrationBuilder.CreateIndex(
                name: "IX_HueLights_HueSceneId",
                table: "HueLights",
                column: "HueSceneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HueLights");

            migrationBuilder.DropTable(
                name: "UpdateTables");

            migrationBuilder.DropTable(
                name: "HueScenes");
        }
    }
}
