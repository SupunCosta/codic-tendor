using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class stockActivity : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "StockHeaderId",
                table: "StockHistoryLog");

            migrationBuilder.RenameColumn(
                name: "WorkFlowTitle",
                table: "StockHistoryLog",
                newName: "DeliveryRequestTime");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayOrder",
                table: "StockActivityType",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-8723-213d2d1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "813a0c70-b58f-433d-8945-9cb166ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-ee367a82addb" }
                });
        }
    }
}
