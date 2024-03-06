using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class contractor1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LotTenderDocs",
                table: "LotTenderDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotTenderAward",
                table: "LotTenderAward");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotTechInstructionsDocs",
                table: "LotTechInstructionsDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotTechDocs",
                table: "LotTechDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotStatus",
                table: "LotStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotProvisionalAcceptenceDocs",
                table: "LotProvisionalAcceptenceDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotProductItemType",
                table: "LotProductItemType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotHistoryLog",
                table: "LotHistoryLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotHeader",
                table: "LotHeader");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotFinalDeliveryDocs",
                table: "LotFinalDeliveryDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotFileType",
                table: "LotFileType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotContractorTeamList",
                table: "LotContractorTeamList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LotContractorsList",
                table: "LotContractorsList");

            migrationBuilder.RenameTable(
                name: "LotTenderDocs",
                newName: "ContractorTenderDocs");

            migrationBuilder.RenameTable(
                name: "LotTenderAward",
                newName: "ContractorTenderAward");

            migrationBuilder.RenameTable(
                name: "LotTechInstructionsDocs",
                newName: "ContractorTechInstructionsDocs");

            migrationBuilder.RenameTable(
                name: "LotTechDocs",
                newName: "ContractorTechDocs");

            migrationBuilder.RenameTable(
                name: "LotStatus",
                newName: "ContractorStatus");

            migrationBuilder.RenameTable(
                name: "LotProvisionalAcceptenceDocs",
                newName: "ContractorProvisionalAcceptenceDocs");

            migrationBuilder.RenameTable(
                name: "LotProductItemType",
                newName: "ContractorProductItemType");

            migrationBuilder.RenameTable(
                name: "LotHistoryLog",
                newName: "ContractorHistoryLog");

            migrationBuilder.RenameTable(
                name: "LotHeader",
                newName: "ContractorHeader");

            migrationBuilder.RenameTable(
                name: "LotFinalDeliveryDocs",
                newName: "ContractorFinalDeliveryDocs");

            migrationBuilder.RenameTable(
                name: "LotFileType",
                newName: "ContractorFileType");

            migrationBuilder.RenameTable(
                name: "LotContractorTeamList",
                newName: "ContractorTeamList");

            migrationBuilder.RenameTable(
                name: "LotContractorsList",
                newName: "ContractorsList");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorTenderDocs",
                table: "ContractorTenderDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorTenderAward",
                table: "ContractorTenderAward",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorTechInstructionsDocs",
                table: "ContractorTechInstructionsDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorTechDocs",
                table: "ContractorTechDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorStatus",
                table: "ContractorStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorProvisionalAcceptenceDocs",
                table: "ContractorProvisionalAcceptenceDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorProductItemType",
                table: "ContractorProductItemType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorHistoryLog",
                table: "ContractorHistoryLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorHeader",
                table: "ContractorHeader",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorFinalDeliveryDocs",
                table: "ContractorFinalDeliveryDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorFileType",
                table: "ContractorFileType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorTeamList",
                table: "ContractorTeamList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractorsList",
                table: "ContractorsList",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorTenderDocs",
                table: "ContractorTenderDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorTenderAward",
                table: "ContractorTenderAward");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorTechInstructionsDocs",
                table: "ContractorTechInstructionsDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorTechDocs",
                table: "ContractorTechDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorTeamList",
                table: "ContractorTeamList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorStatus",
                table: "ContractorStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorsList",
                table: "ContractorsList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorProvisionalAcceptenceDocs",
                table: "ContractorProvisionalAcceptenceDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorProductItemType",
                table: "ContractorProductItemType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorHistoryLog",
                table: "ContractorHistoryLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorHeader",
                table: "ContractorHeader");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorFinalDeliveryDocs",
                table: "ContractorFinalDeliveryDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractorFileType",
                table: "ContractorFileType");

            migrationBuilder.RenameTable(
                name: "ContractorTenderDocs",
                newName: "LotTenderDocs");

            migrationBuilder.RenameTable(
                name: "ContractorTenderAward",
                newName: "LotTenderAward");

            migrationBuilder.RenameTable(
                name: "ContractorTechInstructionsDocs",
                newName: "LotTechInstructionsDocs");

            migrationBuilder.RenameTable(
                name: "ContractorTechDocs",
                newName: "LotTechDocs");

            migrationBuilder.RenameTable(
                name: "ContractorTeamList",
                newName: "LotContractorTeamList");

            migrationBuilder.RenameTable(
                name: "ContractorStatus",
                newName: "LotStatus");

            migrationBuilder.RenameTable(
                name: "ContractorsList",
                newName: "LotContractorsList");

            migrationBuilder.RenameTable(
                name: "ContractorProvisionalAcceptenceDocs",
                newName: "LotProvisionalAcceptenceDocs");

            migrationBuilder.RenameTable(
                name: "ContractorProductItemType",
                newName: "LotProductItemType");

            migrationBuilder.RenameTable(
                name: "ContractorHistoryLog",
                newName: "LotHistoryLog");

            migrationBuilder.RenameTable(
                name: "ContractorHeader",
                newName: "LotHeader");

            migrationBuilder.RenameTable(
                name: "ContractorFinalDeliveryDocs",
                newName: "LotFinalDeliveryDocs");

            migrationBuilder.RenameTable(
                name: "ContractorFileType",
                newName: "LotFileType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotTenderDocs",
                table: "LotTenderDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotTenderAward",
                table: "LotTenderAward",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotTechInstructionsDocs",
                table: "LotTechInstructionsDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotTechDocs",
                table: "LotTechDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotContractorTeamList",
                table: "LotContractorTeamList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotStatus",
                table: "LotStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotContractorsList",
                table: "LotContractorsList",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotProvisionalAcceptenceDocs",
                table: "LotProvisionalAcceptenceDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotProductItemType",
                table: "LotProductItemType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotHistoryLog",
                table: "LotHistoryLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotHeader",
                table: "LotHeader",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotFinalDeliveryDocs",
                table: "LotFinalDeliveryDocs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LotFileType",
                table: "LotFileType",
                column: "Id");
        }
    }
}
