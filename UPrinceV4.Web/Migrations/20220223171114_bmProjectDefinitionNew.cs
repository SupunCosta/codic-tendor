using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class bmProjectDefinitionNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TenderEndDate",
                table: "ProjectTime",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TenderStartDate",
                table: "ProjectTime",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectStatus",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectLanguage",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectLanguage", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProjectLanguage",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "iii93-jsjj-fmms-amdm-b7a93ebd1iii", 2, "en", "nl", "tttdkjg4e8d-fhhd-487d-8170-6b91c89fdddttt" });

            migrationBuilder.InsertData(
                table: "ProjectLanguage",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "yyy93-jsjj-fmms-amdm-b7a93ebd1www", 1, "en", "en", "tttdkjg4e8d-fhhd-487d-8170-6b91c89fdddttt" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectLanguage");

            migrationBuilder.DropColumn(
                name: "TenderEndDate",
                table: "ProjectTime");

            migrationBuilder.DropColumn(
                name: "TenderStartDate",
                table: "ProjectTime");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "ProjectStatus",
                table: "ProjectDefinition");
        }
    }
}
