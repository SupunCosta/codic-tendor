using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedParentIdToCpcResourceFamilyLocalizedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "RiskStatusLocalizedData");

            //migrationBuilder.DropTable(
            //    name: "RiskTypeLocalizedData");

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "CpcResourceFamilyLocalizedData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CpcResourceFamilyLocalizedData");

            migrationBuilder.CreateTable(
                name: "RiskStatusLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskStatusLocalizedData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskTypeLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskTypeLocalizedData", x => x.Id);
                });
        }
    }
}
