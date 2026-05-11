using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SidraHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutPageContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainTitleAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MainTitleEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SubTitleAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SubTitleEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IntroTextAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntroTextEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhyChooseTitleAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WhyChooseTitleEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WhyChooseDescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhyChooseDescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatWeOfferTitleAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WhatWeOfferTitleEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WhatWeOfferDescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatWeOfferDescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MissionTitleAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MissionTitleEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MissionDescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MissionDescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhereWeWorkTitleAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WhereWeWorkTitleEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WhereWeWorkDescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhereWeWorkDescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDetete = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DeletedDatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutPageContent", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutPageContent");
        }
    }
}
