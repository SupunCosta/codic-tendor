using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class langTrans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed");

            migrationBuilder.DeleteData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af");

            migrationBuilder.DeleteData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac");

            migrationBuilder.RenameColumn(
                name: "DeliveryRequestTime",
                table: "StockHistoryLog",
                newName: "WorkFlowTitle");

            migrationBuilder.RenameColumn(
                name: "CPCResourceTypeId",
                table: "StockHeader",
                newName: "CPCId");

            migrationBuilder.AddColumn<string>(
                name: "CPCIdVehicle",
                table: "WHHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BorId",
                table: "WFHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeId",
                table: "StockShortCutPane",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockHeaderId",
                table: "StockHistoryLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "StockActivityType",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasChanged",
                table: "POResources",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryRequest",
                table: "POHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TaxonomyId",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "StockActiveType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-hmnb-d27008688bae", 1, "nl", "Picking ", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-fgdd-e40dbe6a51ac3", 2, "nl", "Reserve", "4010e768-3e06-4702-b337-ee367a82addb" }
                });

            migrationBuilder.InsertData(
                table: "StockActivityType",
                columns: new[] { "Id", "ActivityName", "ActivityTypeId", "DisplayOrder", "LanguageCode" },
                values: new object[,]
                {
                    { "4e01a893-0267-48af-inst-b7a93ebd1ccf", "In To The Stock", "7bcb4e8d-8e8c-inst-8170-6b91c89fc3da", 1, "en" },
                    { "4e01a893-0267-48af-outs-b7a93ebd1ccf", "Out To The Stock", "7bcb4e8d-8e8c-outs-8170-6b91c89fc3da", 2, "en" },
                    { "4e01a893-0267-48af-inst-b7a93ebd1cnl", "In Stock", "7bcb4e8d-8e8c-inst-8170-6b91c89fc3da", 1, "nl" },
                    { "4e01a893-0267-48af-outs-b7a93ebd1cnl", "Uit Stock", "7bcb4e8d-8e8c-outs-8170-6b91c89fc3da", 2, "nl" }
                });

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af34",
                column: "TypeId",
                value: "c46c3a26-39a5-42cc-n7k1-89655304eh6 ");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e472-75b3-40c6-ad61-e40dbe6a51ac8",
                columns: new[] { "Name", "TypeId" },
                values: new object[] { "Verbruiksgoederen", "c46c3a26-39a5-42cc-m06g-89655304eh6" });

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e473-75b3-40c6-ad61-e40dbe6a51ac7",
                columns: new[] { "Name", "TypeId" },
                values: new object[] { "Gereedschap", "c46c3a26-39a5-42cc-n9wn-89655304eh6" });

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e474-75b3-40c6-ad61-e40dbe6a51ac6",
                columns: new[] { "Name", "TypeId" },
                values: new object[] { "Materialen", "c46c3a26-39a5-42cc-n7k1-89655304eh6" });

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e475-75b3-40c6-ad61-e40dbe6a51ac5",
                column: "TypeId",
                value: "");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e476-75b3-40c6-ad61-e40dbe6a51ac4",
                column: "TypeId",
                value: "c46c3a26-39a5-42cc-m06g-89655304eh6");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e477-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "TypeId",
                value: "c46c3a26-39a5-42cc-n9wn-89655304eh6 ");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac1",
                column: "TypeId",
                value: "");

            migrationBuilder.InsertData(
                table: "StockStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "12e2d6c5-1ada-4e74-uuhh-ce7fbf10e27c", 2, "nl", "beschikbaar", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "2732cd5a-0941-4c56-ssdd-f5fdca2ab276", 1, "nl", "Niet meer voorradig", "d60aad0b-2e84-482b-ad25-618d80d49477" }
                });

            migrationBuilder.InsertData(
                table: "StockType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-82a9-d27008688b44", 1, "nl", "Materialen", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51a44", 2, "nl", "Gereedschap", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-d27008688ba44", 3, "nl", "Verbruiksgoederen", "94282458-0b40-40a3-b0f9-c2e40344c8f1" }
                });

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae3",
                column: "Name",
                value: "Good Reception");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae6",
                column: "Name",
                value: "Ontvangst goederen");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac5",
                column: "Name",
                value: "Versturen Goederen");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Good Reception");

            migrationBuilder.InsertData(
                table: "WFType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "bdd9e479-75b3-40c6-wert-e40dbe6a51ac3", 2, "nl", "Ontvangst goederen", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-tyur-d27008688bae", 1, "nl", "Versturen Goederen ", "94282458-0b40-40a3-b0f9-c2e40344c8f1" }
                });

            migrationBuilder.UpdateData(
                table: "WHShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae13",
                column: "Name",
                value: "Mobile");

            migrationBuilder.UpdateData(
                table: "WHShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae16",
                column: "Name",
                value: "Bestelwagen");

            migrationBuilder.UpdateData(
                table: "WHShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac12",
                column: "Name",
                value: "Fixed");

            migrationBuilder.UpdateData(
                table: "WHShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac15",
                column: "Name",
                value: "Gebouw");

            migrationBuilder.UpdateData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "Name",
                value: "Open");

            migrationBuilder.UpdateData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "Name",
                value: "Closed");

            migrationBuilder.UpdateData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "Name",
                value: "Open");

            migrationBuilder.UpdateData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "Name",
                value: "Gesloten");

            migrationBuilder.InsertData(
                table: "WHType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "bdd9e479-75b3-40c6-ldmc-e40dbe6a51ac3", 2, "nl", "Bestelwagen", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-hgkf-d27008688bae", 1, "nl", "Gebouw", "94282458-0b40-40a3-b0f9-c2e40344c8f1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockActiveType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-hmnb-d27008688bae");

            migrationBuilder.DeleteData(
                table: "StockActiveType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-fgdd-e40dbe6a51ac3");

            migrationBuilder.DeleteData(
                table: "StockActivityType",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-inst-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "StockActivityType",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-inst-b7a93ebd1cnl");

            migrationBuilder.DeleteData(
                table: "StockActivityType",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-outs-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "StockActivityType",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-outs-b7a93ebd1cnl");

            migrationBuilder.DeleteData(
                table: "StockStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-uuhh-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "StockStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-ssdd-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "StockType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688b44");

            migrationBuilder.DeleteData(
                table: "StockType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688ba44");

            migrationBuilder.DeleteData(
                table: "StockType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51a44");

            migrationBuilder.DeleteData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-tyur-d27008688bae");

            migrationBuilder.DeleteData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-wert-e40dbe6a51ac3");

            migrationBuilder.DeleteData(
                table: "WHType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-hgkf-d27008688bae");

            migrationBuilder.DeleteData(
                table: "WHType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ldmc-e40dbe6a51ac3");

            migrationBuilder.DropColumn(
                name: "CPCIdVehicle",
                table: "WHHeader");

            migrationBuilder.DropColumn(
                name: "BorId",
                table: "WFHeader");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "StockShortCutPane");

            migrationBuilder.DropColumn(
                name: "StockHeaderId",
                table: "StockHistoryLog");

            migrationBuilder.DropColumn(
                name: "HasChanged",
                table: "POResources");

            migrationBuilder.DropColumn(
                name: "DeliveryRequest",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "TaxonomyId",
                table: "POHeader");

            migrationBuilder.RenameColumn(
                name: "WorkFlowTitle",
                table: "StockHistoryLog",
                newName: "DeliveryRequestTime");

            migrationBuilder.RenameColumn(
                name: "CPCId",
                table: "StockHeader",
                newName: "CPCResourceTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayOrder",
                table: "StockActivityType",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e472-75b3-40c6-ad61-e40dbe6a51ac8",
                column: "Name",
                value: "Consumables");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e473-75b3-40c6-ad61-e40dbe6a51ac7",
                column: "Name",
                value: "Tool");

            migrationBuilder.UpdateData(
                table: "StockShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e474-75b3-40c6-ad61-e40dbe6a51ac6",
                column: "Name",
                value: "Material");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae3",
                column: "Name",
                value: "Good Receive");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae6",
                column: "Name",
                value: "Good Receive");

            migrationBuilder.UpdateData(
                table: "WFShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac5",
                column: "Name",
                value: "Good Picking");

            migrationBuilder.UpdateData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3",
                column: "Name",
                value: "Good Receive");

            migrationBuilder.UpdateData(
                table: "WHShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae13",
                column: "Name",
                value: "Tree View");

            migrationBuilder.UpdateData(
                table: "WHShortCutPane",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae16",
                column: "Name",
                value: "Tree View");

            migrationBuilder.UpdateData(
                table: "WHShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac12",
                column: "Name",
                value: "Last");

            migrationBuilder.UpdateData(
                table: "WHShortCutPane",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac15",
                column: "Name",
                value: "Last");

            migrationBuilder.UpdateData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "Name",
                value: "in voorbereiding");

            migrationBuilder.UpdateData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c",
                column: "Name",
                value: "In Development");

            migrationBuilder.UpdateData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276",
                column: "Name",
                value: "Pending Development");

            migrationBuilder.UpdateData(
                table: "WHStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "Name",
                value: "in uitvoering");

            migrationBuilder.InsertData(
                table: "WHStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "813a0c70-b58f-433d-8945-9cb166ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-8723-213d2d1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" }
                });
        }
    }
}
