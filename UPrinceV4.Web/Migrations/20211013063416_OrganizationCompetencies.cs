using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class OrganizationCompetencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryLevel",
                keyColumn: "Id",
                keyValue: "34d9e479-pob3-40c6-ad61-e40dbe6a51ui");

            migrationBuilder.DeleteData(
                table: "CategoryLevel",
                keyColumn: "Id",
                keyValue: "pod9e479-pob3-40c6-ad61-e40dbe6a51ll");

            migrationBuilder.CreateTable(
                name: "OrganizationCompetencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExperienceLevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationCompetencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FishDetailtype = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsChildren = table.Column<bool>(type: "bit", nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostPictures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostPictures", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "VPOrganisationShortcutPane",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48vp-vp5a-b7a93ebd1ccf",
                column: "OrganisationId",
                value: "7bcb4e8d-8c-487d-8170-vp91c89fc3da");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationCompetencies");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "PostComments");

            migrationBuilder.DropTable(
                name: "PostPictures");

            migrationBuilder.InsertData(
                table: "CategoryLevel",
                columns: new[] { "Id", "DisplayOrder", "Image", "IsChildren", "LanguageCode", "LevelId", "Name", "ParentId" },
                values: new object[] { "34d9e479-pob3-40c6-ad61-e40dbe6a51ui", 1, "http//djfjfdllsl.lk", false, "en", "111111", "Freshwater Fish", null });

            migrationBuilder.InsertData(
                table: "CategoryLevel",
                columns: new[] { "Id", "DisplayOrder", "Image", "IsChildren", "LanguageCode", "LevelId", "Name", "ParentId" },
                values: new object[] { "pod9e479-pob3-40c6-ad61-e40dbe6a51ll", 1, "http//guppy.lk", true, "en", "222222", "Guppy", "34d9e479-pob3-40c6-ad61-e40dbe6a51ui" });

            migrationBuilder.UpdateData(
                table: "VPOrganisationShortcutPane",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48vp-vp5a-b7a93ebd1ccf",
                column: "OrganisationId",
                value: "7bcb4e8d-vp8c-487d-8170-vp91c89fc3da");
        }
    }
}
