using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class CiawStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PmolTeamRoleId",
                table: "CiawHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "CiawHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CiawStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CiawStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "77143c60-ff45-4ca2-ciaws-213d2d1c5f5a", "2", "en", "Completed", "7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce" });

            migrationBuilder.InsertData(
                table: "CiawStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[] { "bdd9e479-75b3-40c6-ciaws-e40dbe6a51ac", "1", "en", "Pending", "4010e768-3e06-4702-ciaws-ee367a82addb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CiawStatus");

            migrationBuilder.DropColumn(
                name: "PmolTeamRoleId",
                table: "CiawHeader");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "CiawHeader");
        }
    }
}
