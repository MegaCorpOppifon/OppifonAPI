using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class NameUpdateCalenderToCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaysOff_Calenders_CalenderId",
                table: "DaysOff");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Calenders_CalenderId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkDays_Calenders_CalenderId",
                table: "WorkDays");

            migrationBuilder.DropTable(
                name: "CalenderAppointments");

            migrationBuilder.DropTable(
                name: "Calenders");

            migrationBuilder.RenameColumn(
                name: "CalenderId",
                table: "WorkDays",
                newName: "CalendarId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkDays_CalenderId",
                table: "WorkDays",
                newName: "IX_WorkDays_CalendarId");

            migrationBuilder.RenameColumn(
                name: "CalenderId",
                table: "Users",
                newName: "CalendarId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CalenderId",
                table: "Users",
                newName: "IX_Users_CalendarId");

            migrationBuilder.RenameColumn(
                name: "CalenderId",
                table: "DaysOff",
                newName: "CalendarId");

            migrationBuilder.RenameIndex(
                name: "IX_DaysOff_CalenderId",
                table: "DaysOff",
                newName: "IX_DaysOff_CalendarId");

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DefaultDuration = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarAppointments",
                columns: table => new
                {
                    CalendarId = table.Column<Guid>(nullable: false),
                    AppointmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarAppointments", x => new { x.CalendarId, x.AppointmentId });
                    table.ForeignKey(
                        name: "FK_CalendarAppointments_Appointments_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarAppointments_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DaysOff_Calendars_CalendarId",
                table: "DaysOff",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Calendars_CalendarId",
                table: "Users",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDays_Calendars_CalendarId",
                table: "WorkDays",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaysOff_Calendars_CalendarId",
                table: "DaysOff");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Calendars_CalendarId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkDays_Calendars_CalendarId",
                table: "WorkDays");

            migrationBuilder.DropTable(
                name: "CalendarAppointments");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.RenameColumn(
                name: "CalendarId",
                table: "WorkDays",
                newName: "CalenderId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkDays_CalendarId",
                table: "WorkDays",
                newName: "IX_WorkDays_CalenderId");

            migrationBuilder.RenameColumn(
                name: "CalendarId",
                table: "Users",
                newName: "CalenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CalendarId",
                table: "Users",
                newName: "IX_Users_CalenderId");

            migrationBuilder.RenameColumn(
                name: "CalendarId",
                table: "DaysOff",
                newName: "CalenderId");

            migrationBuilder.RenameIndex(
                name: "IX_DaysOff_CalendarId",
                table: "DaysOff",
                newName: "IX_DaysOff_CalenderId");

            migrationBuilder.CreateTable(
                name: "Calenders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DefaultDuration = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalenderAppointments",
                columns: table => new
                {
                    CalenderId = table.Column<Guid>(nullable: false),
                    AppointmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalenderAppointments", x => new { x.CalenderId, x.AppointmentId });
                    table.ForeignKey(
                        name: "FK_CalenderAppointments_Appointments_CalenderId",
                        column: x => x.CalenderId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalenderAppointments_Calenders_CalenderId",
                        column: x => x.CalenderId,
                        principalTable: "Calenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DaysOff_Calenders_CalenderId",
                table: "DaysOff",
                column: "CalenderId",
                principalTable: "Calenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Calenders_CalenderId",
                table: "Users",
                column: "CalenderId",
                principalTable: "Calenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDays_Calenders_CalenderId",
                table: "WorkDays",
                column: "CalenderId",
                principalTable: "Calenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
