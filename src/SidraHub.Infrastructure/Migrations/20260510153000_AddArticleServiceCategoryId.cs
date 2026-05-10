using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SidraHub.Infrastructure.Migrations
{
    public partial class AddArticleServiceCategoryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceCategoryId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ServiceCategoryId",
                table: "Articles",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ServiceCategory_ServiceCategoryId",
                table: "Articles",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ServiceCategory_ServiceCategoryId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ServiceCategoryId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ServiceCategoryId",
                table: "Articles");
        }
    }
}
