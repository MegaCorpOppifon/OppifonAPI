using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FixRelaitonshipsBetweenExpertAndTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpertTags_Users_ExpertId1",
                table: "ExpertTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpertTags_Tags_TagId1",
                table: "ExpertTags");

            migrationBuilder.DropIndex(
                name: "IX_ExpertTags_ExpertId1",
                table: "ExpertTags");

            migrationBuilder.DropIndex(
                name: "IX_ExpertTags_TagId1",
                table: "ExpertTags");

            migrationBuilder.DropColumn(
                name: "ExpertId1",
                table: "ExpertTags");

            migrationBuilder.DropColumn(
                name: "TagId1",
                table: "ExpertTags");

            migrationBuilder.CreateTable(
                name: "MainFieldTags",
                columns: table => new
                {
                    ExpertId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainFieldTags", x => new { x.ExpertId, x.TagId });
                    table.ForeignKey(
                        name: "FK_MainFieldTags_Users_ExpertId",
                        column: x => x.ExpertId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MainFieldTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainFieldTags_TagId",
                table: "MainFieldTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainFieldTags");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpertId1",
                table: "ExpertTags",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TagId1",
                table: "ExpertTags",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpertTags_ExpertId1",
                table: "ExpertTags",
                column: "ExpertId1");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertTags_TagId1",
                table: "ExpertTags",
                column: "TagId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertTags_Users_ExpertId1",
                table: "ExpertTags",
                column: "ExpertId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpertTags_Tags_TagId1",
                table: "ExpertTags",
                column: "TagId1",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
