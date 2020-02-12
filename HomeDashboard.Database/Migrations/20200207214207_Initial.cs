using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace kriez.HomeDashboard.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

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
                name: "CalendarItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CalendarId = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: true),
                    End = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarItems_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Brightness = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HueLights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HueLights_HueScenes_Group",
                        column: x => x.Group,
                        principalTable: "HueScenes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarItems_CalendarId",
                table: "CalendarItems",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_HueLights_Group",
                table: "HueLights",
                column: "Group");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarItems");

            migrationBuilder.DropTable(
                name: "HueLights");

            migrationBuilder.DropTable(
                name: "UpdateTables");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "HueScenes");
        }
    }
}
