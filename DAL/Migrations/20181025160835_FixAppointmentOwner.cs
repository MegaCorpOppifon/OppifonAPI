using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FixAppointmentOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_OwnerId",
                table: "Appointments");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Appointments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Calendars_OwnerId",
                table: "Appointments",
                column: "OwnerId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Calendars_OwnerId",
                table: "Appointments");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Appointments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_OwnerId",
                table: "Appointments",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
