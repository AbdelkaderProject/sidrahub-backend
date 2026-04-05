using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SidraHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ServiceRequest");

            migrationBuilder.AddColumn<int>(
                name: "CompanyProfileId",
                table: "TeamMember",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ServiceRequest",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "ServicePackage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceCategoryId",
                table: "Service",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyProfileId",
                table: "Partners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyProfileId",
                table: "Branche",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDetete = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDetete = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_CompanyProfileId",
                table: "TeamMember",
                column: "CompanyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequest_UserId",
                table: "ServiceRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackage_ServiceId",
                table: "ServicePackage",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ServiceCategoryId",
                table: "Service",
                column: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_CompanyProfileId",
                table: "Partners",
                column: "CompanyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Branche_CompanyProfileId",
                table: "Branche",
                column: "CompanyProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branche_CompanyProfile_CompanyProfileId",
                table: "Branche",
                column: "CompanyProfileId",
                principalTable: "CompanyProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_CompanyProfile_CompanyProfileId",
                table: "Partners",
                column: "CompanyProfileId",
                principalTable: "CompanyProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ServiceCategory_ServiceCategoryId",
                table: "Service",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePackage_Service_ServiceId",
                table: "ServicePackage",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequest_Users_UserId",
                table: "ServiceRequest",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMember_CompanyProfile_CompanyProfileId",
                table: "TeamMember",
                column: "CompanyProfileId",
                principalTable: "CompanyProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branche_CompanyProfile_CompanyProfileId",
                table: "Branche");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_CompanyProfile_CompanyProfileId",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_ServiceCategory_ServiceCategoryId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePackage_Service_ServiceId",
                table: "ServicePackage");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequest_Users_UserId",
                table: "ServiceRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMember_CompanyProfile_CompanyProfileId",
                table: "TeamMember");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_TeamMember_CompanyProfileId",
                table: "TeamMember");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequest_UserId",
                table: "ServiceRequest");

            migrationBuilder.DropIndex(
                name: "IX_ServicePackage_ServiceId",
                table: "ServicePackage");

            migrationBuilder.DropIndex(
                name: "IX_Service_ServiceCategoryId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Partners_CompanyProfileId",
                table: "Partners");

            migrationBuilder.DropIndex(
                name: "IX_Branche_CompanyProfileId",
                table: "Branche");

            migrationBuilder.DropColumn(
                name: "CompanyProfileId",
                table: "TeamMember");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ServiceRequest");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "ServicePackage");

            migrationBuilder.DropColumn(
                name: "ServiceCategoryId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "CompanyProfileId",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "CompanyProfileId",
                table: "Branche");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "ServiceRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
