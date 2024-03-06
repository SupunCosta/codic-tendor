using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class CabCertificationUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractingUnit",
                table: "CiawHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificationTitle",
                table: "CabCertification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificationUrl",
                table: "CabCertification",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractingUnit",
                table: "CiawHeader");

            migrationBuilder.DropColumn(
                name: "CertificationTitle",
                table: "CabCertification");

            migrationBuilder.DropColumn(
                name: "CertificationUrl",
                table: "CabCertification");
        }
    }
}
