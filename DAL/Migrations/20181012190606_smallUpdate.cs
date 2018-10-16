using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class smallUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpertTags_Experts_ExpertId",
                table: "ExpertTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpertTags_Experts_ExpertId1",
                table: "ExpertTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Experts_ExpertId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Experts");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExpertCategoryId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExpertCategoryId",
                table: "Users",
                column: "ExpertCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertTags_Users_ExpertId",
                table: "ExpertTags",
                column: "ExpertId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertTags_Users_ExpertId1",
                table: "ExpertTags",
                column: "ExpertId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ExpertId",
                table: "Reviews",
                column: "ExpertId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Categories_ExpertCategoryId",
                table: "Users",
                column: "ExpertCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpertTags_Users_ExpertId",
                table: "ExpertTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpertTags_Users_ExpertId1",
                table: "ExpertTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ExpertId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Categories_ExpertCategoryId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ExpertCategoryId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExpertCategoryId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Experts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ExpertCategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experts_Categories_ExpertCategoryId",
                        column: x => x.ExpertCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Experts_ExpertCategoryId",
                table: "Experts",
                column: "ExpertCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertTags_Experts_ExpertId",
                table: "ExpertTags",
                column: "ExpertId",
                principalTable: "Experts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertTags_Experts_ExpertId1",
                table: "ExpertTags",
                column: "ExpertId1",
                principalTable: "Experts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Experts_ExpertId",
                table: "Reviews",
                column: "ExpertId",
                principalTable: "Experts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
