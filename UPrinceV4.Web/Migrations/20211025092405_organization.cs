using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class organization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationTaxonomyId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationTaxonomyLevelId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTaxonomy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationTaxonomyLevel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsChildren = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationTaxonomyLevel", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrganizationTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[,]
                {
                    { "vv5ab9fe-po57-4088-82a9-d27008688bbb", 1, false, "en", "qq282458-0b40-poa3-b0f9-c2e40344c8kk", "Organization" },
                    { "uud9e479-pob3-40c6-ad61-e40dbe6a5111", 2, true, "en", "2210e768-3e06-po02-b337-ee367a82adjj", "CU" },
                    { "gg5ab9fe-po57-4088-82a9-d27008688ttt", 1, false, "nl", "qq282458-0b40-poa3-b0f9-c2e40344c8kk", "Organization nl" },
                    { "kkd9e479-pob3-40c6-ad61-e40dbe6a5444", 2, true, "nl", "2210e768-3e06-po02-b337-ee367a82adjj", "CU nl" },
                    { "ttkab9fe-po57-4088-82a9-d27008688bbb", 3, true, "en", "oo10e768-3e06-po02-b337-ee367a82admn", "BU" },
                    { "eew9e479-pob3-40c6-ad61-e40dbe6a5111", 3, true, "nl", "oo10e768-3e06-po02-b337-ee367a82admn", "BU nl" },
                    { "jvfkab9fe-po57-4088-82a9-d27008688jvf", 4, true, "en", "1210e768-3e06-po02-b337-ee367a82ad12", "Department" },
                    { "nbv9e479-pob3-40c6-ad61-e40dbe6a5nbv", 4, true, "nl", "1210e768-3e06-po02-b337-ee367a82ad12", "Department nl" },
                    { "slaab9fe-po57-4088-82a9-d27008688kgd", 5, true, "en", "fg10e768-3e06-po02-b337-ee367a82adfg", "Team" },
                    { "qwe9e479-pob3-40c6-ad61-e40dbe6a5lks", 5, true, "nl", "fg10e768-3e06-po02-b337-ee367a82adfg", "Team nl" },
                    { "aqwab9fe-po57-4088-82a9-d27008688mvk", 6, true, "en", "we10e768-3e06-po02-b337-ee367a82adwe", "Person Search" },
                    { "bds9e479-pob3-40c6-ad61-e40dbe6a5gtu", 6, true, "nl", "we10e768-3e06-po02-b337-ee367a82adwe", "Person Search nl" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "OrganizationTaxonomy");

            migrationBuilder.DropTable(
                name: "OrganizationTaxonomyLevel");
        }
    }
}
