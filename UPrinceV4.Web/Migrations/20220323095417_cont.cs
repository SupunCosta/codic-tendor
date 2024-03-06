using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class cont : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "ContractorAccreditation",
                newName: "experienceName");

            migrationBuilder.AddColumn<string>(
                name: "CabPersonId",
                table: "ContractorSupplierList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CabPersonName",
                table: "ContractorSupplierList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CabPersonId",
                table: "ContractorAccreditation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CabPersonName",
                table: "ContractorAccreditation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExperienceId",
                table: "ContractorAccreditation",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CabPersonId",
                table: "ContractorSupplierList");

            migrationBuilder.DropColumn(
                name: "CabPersonName",
                table: "ContractorSupplierList");

            migrationBuilder.DropColumn(
                name: "CabPersonId",
                table: "ContractorAccreditation");

            migrationBuilder.DropColumn(
                name: "CabPersonName",
                table: "ContractorAccreditation");

            migrationBuilder.DropColumn(
                name: "ExperienceId",
                table: "ContractorAccreditation");

            migrationBuilder.RenameColumn(
                name: "experienceName",
                table: "ContractorAccreditation",
                newName: "Experience");
        }
    }
}
