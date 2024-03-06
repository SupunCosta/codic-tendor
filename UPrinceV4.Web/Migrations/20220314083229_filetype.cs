using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class filetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ContractorTenderDocs",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ContractorTechInstructionsDocs",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ContractorTechDocs",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ContractorProvisionalAcceptenceDocs",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ContractorFinalDeliveryDocs",
                newName: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "ContractorTenderDocs",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "ContractorTechInstructionsDocs",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "ContractorTechDocs",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "ContractorProvisionalAcceptenceDocs",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "ContractorFinalDeliveryDocs",
                newName: "Type");
        }
    }
}
