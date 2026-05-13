using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SidraHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ArticlesComment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ArticlesComment");
        }
    }
}
