using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POConsumables_Bor_Bor_Id",
                table: "POConsumables");

            migrationBuilder.DropForeignKey(
                name: "FK_POConsumables_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POConsumables");

            migrationBuilder.DropForeignKey(
                name: "FK_POConsumables_POHeader_PurchesOrder_Id",
                table: "POConsumables");

            migrationBuilder.DropForeignKey(
                name: "FK_POExternalProducts_Bor_BOR_Id",
                table: "POExternalProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_POExternalProducts_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POExternalProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_POExternalProducts_POHeader_PurchesOrder_Id",
                table: "POExternalProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_Customer_CabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_Supplier_CabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_POHeader_PurchesOrder_Id",
                table: "POInvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_POLobours_Bor_BOR_Id",
                table: "POLobours");

            migrationBuilder.DropForeignKey(
                name: "FK_POLobours_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POLobours");

            migrationBuilder.DropForeignKey(
                name: "FK_POLobours_POHeader_PurchesOrder_Id",
                table: "POLobours");

            migrationBuilder.DropForeignKey(
                name: "FK_POMaterials_Bor_BOR_Id",
                table: "POMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_POMaterials_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_POMaterials_POHeader_PurchesOrder_Id",
                table: "POMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_POTools_Bor_BOR_Id",
                table: "POTools");

            migrationBuilder.DropForeignKey(
                name: "FK_POTools_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POTools");

            migrationBuilder.DropForeignKey(
                name: "FK_POTools_POHeader_PurchesOrder_Id",
                table: "POTools");

            migrationBuilder.RenameColumn(
                name: "Unit_Price",
                table: "POTools",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "Total_Price",
                table: "POTools",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "Resource_Title",
                table: "POTools",
                newName: "ResourceTitle");

            migrationBuilder.RenameColumn(
                name: "PurchesOrder_Id",
                table: "POTools",
                newName: "PurchesOrderId");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasure_Id",
                table: "POTools",
                newName: "CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameColumn(
                name: "BOR_Id",
                table: "POTools",
                newName: "BORId");

            migrationBuilder.RenameIndex(
                name: "IX_POTools_PurchesOrder_Id",
                table: "POTools",
                newName: "IX_POTools_PurchesOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_POTools_CpcBasicUnitOfMeasure_Id",
                table: "POTools",
                newName: "IX_POTools_CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_POTools_BOR_Id",
                table: "POTools",
                newName: "IX_POTools_BORId");

            migrationBuilder.RenameColumn(
                name: "Unit_Price",
                table: "POMaterials",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "Total_Price",
                table: "POMaterials",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "Resource_Title",
                table: "POMaterials",
                newName: "ResourceTitle");

            migrationBuilder.RenameColumn(
                name: "PurchesOrder_Id",
                table: "POMaterials",
                newName: "PurchesOrderId");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasure_Id",
                table: "POMaterials",
                newName: "CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameColumn(
                name: "BOR_Id",
                table: "POMaterials",
                newName: "BORId");

            migrationBuilder.RenameIndex(
                name: "IX_POMaterials_PurchesOrder_Id",
                table: "POMaterials",
                newName: "IX_POMaterials_PurchesOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_POMaterials_CpcBasicUnitOfMeasure_Id",
                table: "POMaterials",
                newName: "IX_POMaterials_CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_POMaterials_BOR_Id",
                table: "POMaterials",
                newName: "IX_POMaterials_BORId");

            migrationBuilder.RenameColumn(
                name: "Unit_Price",
                table: "POLobours",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "Total_Price",
                table: "POLobours",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "Resource_Title",
                table: "POLobours",
                newName: "ResourceTitle");

            migrationBuilder.RenameColumn(
                name: "PurchesOrder_Id",
                table: "POLobours",
                newName: "PurchesOrderId");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasure_Id",
                table: "POLobours",
                newName: "CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameColumn(
                name: "BOR_Id",
                table: "POLobours",
                newName: "BORId");

            migrationBuilder.RenameIndex(
                name: "IX_POLobours_PurchesOrder_Id",
                table: "POLobours",
                newName: "IX_POLobours_PurchesOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_POLobours_CpcBasicUnitOfMeasure_Id",
                table: "POLobours",
                newName: "IX_POLobours_CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_POLobours_BOR_Id",
                table: "POLobours",
                newName: "IX_POLobours_BORId");

            migrationBuilder.RenameColumn(
                name: "Supplier_Reference",
                table: "POInvolvedParties",
                newName: "SupplierReference");

            migrationBuilder.RenameColumn(
                name: "Supplier_CabPersonCompanyId",
                table: "POInvolvedParties",
                newName: "SupplierCabPersonCompanyId");

            migrationBuilder.RenameColumn(
                name: "PurchesOrder_Id",
                table: "POInvolvedParties",
                newName: "PurchesOrderId");

            migrationBuilder.RenameColumn(
                name: "Customer_Reference",
                table: "POInvolvedParties",
                newName: "CustomerReference");

            migrationBuilder.RenameColumn(
                name: "Customer_CabPersonCompanyId",
                table: "POInvolvedParties",
                newName: "CustomerCabPersonCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_POInvolvedParties_Supplier_CabPersonCompanyId",
                table: "POInvolvedParties",
                newName: "IX_POInvolvedParties_SupplierCabPersonCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_POInvolvedParties_PurchesOrder_Id",
                table: "POInvolvedParties",
                newName: "IX_POInvolvedParties_PurchesOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_POInvolvedParties_Customer_CabPersonCompanyId",
                table: "POInvolvedParties",
                newName: "IX_POInvolvedParties_CustomerCabPersonCompanyId");

            migrationBuilder.RenameColumn(
                name: "Unit_Price",
                table: "POExternalProducts",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "Transaction_Id",
                table: "POExternalProducts",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "Total_Price",
                table: "POExternalProducts",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "Resource_Title",
                table: "POExternalProducts",
                newName: "ResourceTitle");

            migrationBuilder.RenameColumn(
                name: "Requested_Date",
                table: "POExternalProducts",
                newName: "RequestedDate");

            migrationBuilder.RenameColumn(
                name: "PurchesOrder_Id",
                table: "POExternalProducts",
                newName: "PurchesOrderId");

            migrationBuilder.RenameColumn(
                name: "Project_Title",
                table: "POExternalProducts",
                newName: "ProjectTitle");

            migrationBuilder.RenameColumn(
                name: "Product_Title",
                table: "POExternalProducts",
                newName: "ProductTitle");

            migrationBuilder.RenameColumn(
                name: "Cross_Reference",
                table: "POExternalProducts",
                newName: "CrossReference");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasure_Id",
                table: "POExternalProducts",
                newName: "CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameColumn(
                name: "BOR_Id",
                table: "POExternalProducts",
                newName: "BORId");

            migrationBuilder.RenameIndex(
                name: "IX_POExternalProducts_PurchesOrder_Id",
                table: "POExternalProducts",
                newName: "IX_POExternalProducts_PurchesOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_POExternalProducts_CpcBasicUnitOfMeasure_Id",
                table: "POExternalProducts",
                newName: "IX_POExternalProducts_CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_POExternalProducts_BOR_Id",
                table: "POExternalProducts",
                newName: "IX_POExternalProducts_BORId");

            migrationBuilder.RenameColumn(
                name: "Unit_Price",
                table: "POConsumables",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "Total_Price",
                table: "POConsumables",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "Resource_Title",
                table: "POConsumables",
                newName: "ResourceTitle");

            migrationBuilder.RenameColumn(
                name: "PurchesOrder_Id",
                table: "POConsumables",
                newName: "PurchesOrderId");

            migrationBuilder.RenameColumn(
                name: "Project_Title",
                table: "POConsumables",
                newName: "ProjectTitle");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasure_Id",
                table: "POConsumables",
                newName: "CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameColumn(
                name: "Bor_Id",
                table: "POConsumables",
                newName: "BorId");

            migrationBuilder.RenameIndex(
                name: "IX_POConsumables_PurchesOrder_Id",
                table: "POConsumables",
                newName: "IX_POConsumables_PurchesOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_POConsumables_CpcBasicUnitOfMeasure_Id",
                table: "POConsumables",
                newName: "IX_POConsumables_CpcBasicUnitOfMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_POConsumables_Bor_Id",
                table: "POConsumables",
                newName: "IX_POConsumables_BorId");

            migrationBuilder.AddForeignKey(
                name: "FK_POConsumables_Bor_BorId",
                table: "POConsumables",
                column: "BorId",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POConsumables_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POConsumables",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POConsumables_POHeader_PurchesOrderId",
                table: "POConsumables",
                column: "PurchesOrderId",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POExternalProducts_Bor_BORId",
                table: "POExternalProducts",
                column: "BORId",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POExternalProducts_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POExternalProducts",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POExternalProducts_POHeader_PurchesOrderId",
                table: "POExternalProducts",
                column: "PurchesOrderId",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_CustomerCabPersonCompanyId",
                table: "POInvolvedParties",
                column: "CustomerCabPersonCompanyId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_SupplierCabPersonCompanyId",
                table: "POInvolvedParties",
                column: "SupplierCabPersonCompanyId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_POHeader_PurchesOrderId",
                table: "POInvolvedParties",
                column: "PurchesOrderId",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POLobours_Bor_BORId",
                table: "POLobours",
                column: "BORId",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POLobours_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POLobours",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POLobours_POHeader_PurchesOrderId",
                table: "POLobours",
                column: "PurchesOrderId",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POMaterials_Bor_BORId",
                table: "POMaterials",
                column: "BORId",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POMaterials_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POMaterials",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POMaterials_POHeader_PurchesOrderId",
                table: "POMaterials",
                column: "PurchesOrderId",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POTools_Bor_BORId",
                table: "POTools",
                column: "BORId",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POTools_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POTools",
                column: "CpcBasicUnitOfMeasureId",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POTools_POHeader_PurchesOrderId",
                table: "POTools",
                column: "PurchesOrderId",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POConsumables_Bor_BorId",
                table: "POConsumables");

            migrationBuilder.DropForeignKey(
                name: "FK_POConsumables_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POConsumables");

            migrationBuilder.DropForeignKey(
                name: "FK_POConsumables_POHeader_PurchesOrderId",
                table: "POConsumables");

            migrationBuilder.DropForeignKey(
                name: "FK_POExternalProducts_Bor_BORId",
                table: "POExternalProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_POExternalProducts_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POExternalProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_POExternalProducts_POHeader_PurchesOrderId",
                table: "POExternalProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_CustomerCabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_SupplierCabPersonCompanyId",
                table: "POInvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_POInvolvedParties_POHeader_PurchesOrderId",
                table: "POInvolvedParties");

            migrationBuilder.DropForeignKey(
                name: "FK_POLobours_Bor_BORId",
                table: "POLobours");

            migrationBuilder.DropForeignKey(
                name: "FK_POLobours_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POLobours");

            migrationBuilder.DropForeignKey(
                name: "FK_POLobours_POHeader_PurchesOrderId",
                table: "POLobours");

            migrationBuilder.DropForeignKey(
                name: "FK_POMaterials_Bor_BORId",
                table: "POMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_POMaterials_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_POMaterials_POHeader_PurchesOrderId",
                table: "POMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_POTools_Bor_BORId",
                table: "POTools");

            migrationBuilder.DropForeignKey(
                name: "FK_POTools_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasureId",
                table: "POTools");

            migrationBuilder.DropForeignKey(
                name: "FK_POTools_POHeader_PurchesOrderId",
                table: "POTools");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "POTools",
                newName: "Unit_Price");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "POTools",
                newName: "Total_Price");

            migrationBuilder.RenameColumn(
                name: "ResourceTitle",
                table: "POTools",
                newName: "Resource_Title");

            migrationBuilder.RenameColumn(
                name: "PurchesOrderId",
                table: "POTools",
                newName: "PurchesOrder_Id");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasureId",
                table: "POTools",
                newName: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameColumn(
                name: "BORId",
                table: "POTools",
                newName: "BOR_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POTools_PurchesOrderId",
                table: "POTools",
                newName: "IX_POTools_PurchesOrder_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POTools_CpcBasicUnitOfMeasureId",
                table: "POTools",
                newName: "IX_POTools_CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POTools_BORId",
                table: "POTools",
                newName: "IX_POTools_BOR_Id");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "POMaterials",
                newName: "Unit_Price");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "POMaterials",
                newName: "Total_Price");

            migrationBuilder.RenameColumn(
                name: "ResourceTitle",
                table: "POMaterials",
                newName: "Resource_Title");

            migrationBuilder.RenameColumn(
                name: "PurchesOrderId",
                table: "POMaterials",
                newName: "PurchesOrder_Id");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasureId",
                table: "POMaterials",
                newName: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameColumn(
                name: "BORId",
                table: "POMaterials",
                newName: "BOR_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POMaterials_PurchesOrderId",
                table: "POMaterials",
                newName: "IX_POMaterials_PurchesOrder_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POMaterials_CpcBasicUnitOfMeasureId",
                table: "POMaterials",
                newName: "IX_POMaterials_CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POMaterials_BORId",
                table: "POMaterials",
                newName: "IX_POMaterials_BOR_Id");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "POLobours",
                newName: "Unit_Price");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "POLobours",
                newName: "Total_Price");

            migrationBuilder.RenameColumn(
                name: "ResourceTitle",
                table: "POLobours",
                newName: "Resource_Title");

            migrationBuilder.RenameColumn(
                name: "PurchesOrderId",
                table: "POLobours",
                newName: "PurchesOrder_Id");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasureId",
                table: "POLobours",
                newName: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameColumn(
                name: "BORId",
                table: "POLobours",
                newName: "BOR_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POLobours_PurchesOrderId",
                table: "POLobours",
                newName: "IX_POLobours_PurchesOrder_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POLobours_CpcBasicUnitOfMeasureId",
                table: "POLobours",
                newName: "IX_POLobours_CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POLobours_BORId",
                table: "POLobours",
                newName: "IX_POLobours_BOR_Id");

            migrationBuilder.RenameColumn(
                name: "SupplierReference",
                table: "POInvolvedParties",
                newName: "Supplier_Reference");

            migrationBuilder.RenameColumn(
                name: "SupplierCabPersonCompanyId",
                table: "POInvolvedParties",
                newName: "Supplier_CabPersonCompanyId");

            migrationBuilder.RenameColumn(
                name: "PurchesOrderId",
                table: "POInvolvedParties",
                newName: "PurchesOrder_Id");

            migrationBuilder.RenameColumn(
                name: "CustomerReference",
                table: "POInvolvedParties",
                newName: "Customer_Reference");

            migrationBuilder.RenameColumn(
                name: "CustomerCabPersonCompanyId",
                table: "POInvolvedParties",
                newName: "Customer_CabPersonCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_POInvolvedParties_SupplierCabPersonCompanyId",
                table: "POInvolvedParties",
                newName: "IX_POInvolvedParties_Supplier_CabPersonCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_POInvolvedParties_PurchesOrderId",
                table: "POInvolvedParties",
                newName: "IX_POInvolvedParties_PurchesOrder_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POInvolvedParties_CustomerCabPersonCompanyId",
                table: "POInvolvedParties",
                newName: "IX_POInvolvedParties_Customer_CabPersonCompanyId");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "POExternalProducts",
                newName: "Unit_Price");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "POExternalProducts",
                newName: "Transaction_Id");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "POExternalProducts",
                newName: "Total_Price");

            migrationBuilder.RenameColumn(
                name: "ResourceTitle",
                table: "POExternalProducts",
                newName: "Resource_Title");

            migrationBuilder.RenameColumn(
                name: "RequestedDate",
                table: "POExternalProducts",
                newName: "Requested_Date");

            migrationBuilder.RenameColumn(
                name: "PurchesOrderId",
                table: "POExternalProducts",
                newName: "PurchesOrder_Id");

            migrationBuilder.RenameColumn(
                name: "ProjectTitle",
                table: "POExternalProducts",
                newName: "Project_Title");

            migrationBuilder.RenameColumn(
                name: "ProductTitle",
                table: "POExternalProducts",
                newName: "Product_Title");

            migrationBuilder.RenameColumn(
                name: "CrossReference",
                table: "POExternalProducts",
                newName: "Cross_Reference");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasureId",
                table: "POExternalProducts",
                newName: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameColumn(
                name: "BORId",
                table: "POExternalProducts",
                newName: "BOR_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POExternalProducts_PurchesOrderId",
                table: "POExternalProducts",
                newName: "IX_POExternalProducts_PurchesOrder_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POExternalProducts_CpcBasicUnitOfMeasureId",
                table: "POExternalProducts",
                newName: "IX_POExternalProducts_CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POExternalProducts_BORId",
                table: "POExternalProducts",
                newName: "IX_POExternalProducts_BOR_Id");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "POConsumables",
                newName: "Unit_Price");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "POConsumables",
                newName: "Total_Price");

            migrationBuilder.RenameColumn(
                name: "ResourceTitle",
                table: "POConsumables",
                newName: "Resource_Title");

            migrationBuilder.RenameColumn(
                name: "PurchesOrderId",
                table: "POConsumables",
                newName: "PurchesOrder_Id");

            migrationBuilder.RenameColumn(
                name: "ProjectTitle",
                table: "POConsumables",
                newName: "Project_Title");

            migrationBuilder.RenameColumn(
                name: "CpcBasicUnitOfMeasureId",
                table: "POConsumables",
                newName: "CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameColumn(
                name: "BorId",
                table: "POConsumables",
                newName: "Bor_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POConsumables_PurchesOrderId",
                table: "POConsumables",
                newName: "IX_POConsumables_PurchesOrder_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POConsumables_CpcBasicUnitOfMeasureId",
                table: "POConsumables",
                newName: "IX_POConsumables_CpcBasicUnitOfMeasure_Id");

            migrationBuilder.RenameIndex(
                name: "IX_POConsumables_BorId",
                table: "POConsumables",
                newName: "IX_POConsumables_Bor_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_POConsumables_Bor_Bor_Id",
                table: "POConsumables",
                column: "Bor_Id",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POConsumables_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POConsumables",
                column: "CpcBasicUnitOfMeasure_Id",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POConsumables_POHeader_PurchesOrder_Id",
                table: "POConsumables",
                column: "PurchesOrder_Id",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POExternalProducts_Bor_BOR_Id",
                table: "POExternalProducts",
                column: "BOR_Id",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POExternalProducts_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POExternalProducts",
                column: "CpcBasicUnitOfMeasure_Id",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POExternalProducts_POHeader_PurchesOrder_Id",
                table: "POExternalProducts",
                column: "PurchesOrder_Id",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_Customer_CabPersonCompanyId",
                table: "POInvolvedParties",
                column: "Customer_CabPersonCompanyId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_CabPersonCompany_Supplier_CabPersonCompanyId",
                table: "POInvolvedParties",
                column: "Supplier_CabPersonCompanyId",
                principalTable: "CabPersonCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POInvolvedParties_POHeader_PurchesOrder_Id",
                table: "POInvolvedParties",
                column: "PurchesOrder_Id",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POLobours_Bor_BOR_Id",
                table: "POLobours",
                column: "BOR_Id",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POLobours_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POLobours",
                column: "CpcBasicUnitOfMeasure_Id",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POLobours_POHeader_PurchesOrder_Id",
                table: "POLobours",
                column: "PurchesOrder_Id",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POMaterials_Bor_BOR_Id",
                table: "POMaterials",
                column: "BOR_Id",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POMaterials_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POMaterials",
                column: "CpcBasicUnitOfMeasure_Id",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POMaterials_POHeader_PurchesOrder_Id",
                table: "POMaterials",
                column: "PurchesOrder_Id",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POTools_Bor_BOR_Id",
                table: "POTools",
                column: "BOR_Id",
                principalTable: "Bor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POTools_CpcBasicUnitOfMeasure_CpcBasicUnitOfMeasure_Id",
                table: "POTools",
                column: "CpcBasicUnitOfMeasure_Id",
                principalTable: "CpcBasicUnitOfMeasure",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POTools_POHeader_PurchesOrder_Id",
                table: "POTools",
                column: "PurchesOrder_Id",
                principalTable: "POHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
