using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FixAppointmentAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Appointments_AppointmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AppointmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "Calendars",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Appointments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_AppointmentId",
                table: "Calendars",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OwnerId",
                table: "Appointments",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_OwnerId",
                table: "Appointments",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_Appointments_AppointmentId",
                table: "Calendars",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_OwnerId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Calendars_Appointments_AppointmentId",
                table: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_Calendars_AppointmentId",
                table: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_OwnerId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Appointments");

            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AppointmentId",
                table: "Users",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Appointments_AppointmentId",
                table: "Users",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
