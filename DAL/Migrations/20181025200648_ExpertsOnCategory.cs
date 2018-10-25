using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ExpertsOnCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Categories_ExpertCategoryId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Categories_ExpertCategoryId",
                table: "Users",
                column: "ExpertCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Categories_ExpertCategoryId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Categories_ExpertCategoryId",
                table: "Users",
                column: "ExpertCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
