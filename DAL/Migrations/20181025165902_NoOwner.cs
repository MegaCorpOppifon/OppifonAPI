using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class NoOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Calendars_OwnerId",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "Calendars",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Appointments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_AppointmentId",
                table: "Calendars",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OwnerId",
                table: "Appointments",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Calendars_OwnerId",
                table: "Appointments",
                column: "OwnerId",
                principalTable: "Calendars",
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
    }
}
