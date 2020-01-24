using Microsoft.EntityFrameworkCore.Migrations;

namespace kriez.HomeDashboard.Data.Migrations
{
    public partial class AddedRelationshipToCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CalendarId",
                table: "CalendarItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalendarItems_CalendarId",
                table: "CalendarItems",
                column: "CalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarItems_Calendars_CalendarId",
                table: "CalendarItems",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarItems_Calendars_CalendarId",
                table: "CalendarItems");

            migrationBuilder.DropIndex(
                name: "IX_CalendarItems_CalendarId",
                table: "CalendarItems");

            migrationBuilder.DropColumn(
                name: "CalendarId",
                table: "CalendarItems");
        }
    }
}
