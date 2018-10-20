using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FixCalendarAppointmentIdIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarAppointments_Appointments_CalendarId",
                table: "CalendarAppointments");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarAppointments_AppointmentId",
                table: "CalendarAppointments",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarAppointments_Appointments_AppointmentId",
                table: "CalendarAppointments",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarAppointments_Appointments_AppointmentId",
                table: "CalendarAppointments");

            migrationBuilder.DropIndex(
                name: "IX_CalendarAppointments_AppointmentId",
                table: "CalendarAppointments");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarAppointments_Appointments_CalendarId",
                table: "CalendarAppointments",
                column: "CalendarId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
