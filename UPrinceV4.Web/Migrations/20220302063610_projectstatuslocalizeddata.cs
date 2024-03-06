using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class projectstatuslocalizeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentBudgetSpent",
                table: "ProjectFinance",
                newName: "CustomerBudgetSpent");

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagerId",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ProjectStateLocalizedData",
                columns: new[] { "Id", "Label", "LanguageCode", "ProjectStateId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-657dd897f5bb", "in voorbereiding", "nl", "1e276a41-26af-4340-b4bc-c9aeb107efe7" },
                    { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", "In Development", "en", "65f2f0fd-ea7a-440e-9fd4-346628ef1299" },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab276", "Pending Development", "en", "1e276a41-26af-4340-b4bc-c9aeb107efe7" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", "goedgekeurd", "nl", "624b5481-fb27-44dc-9aae-5a5c85d59720" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed", "ter goedkeuring", "nl", "152fa5c2-e697-451a-bf3d-fa342ea95670" },
                    { "77143c60-ff45-4ca2-8723-213d2d1c5f5a", "Approved", "en", "624b5481-fb27-44dc-9aae-5a5c85d59720" },
                    { "813a0c70-b58f-433d-8945-9cb166ae42af", "In Review", "en", "152fa5c2-e697-451a-bf3d-fa342ea95670" },
                    { "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", "Handed Over", "en", "d7e13082-77f4-44ad-8ad8-b0b9dad94ac1" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", "in uitvoering", "nl", "65f2f0fd-ea7a-440e-9fd4-346628ef1299" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", "afgewerkt en doorgegeven", "nl", "d7e13082-77f4-44ad-8ad8-b0b9dad94ac1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb");

            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed");

            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af");

            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "ProjectStateLocalizedData",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "ProjectDefinition");

            migrationBuilder.RenameColumn(
                name: "CustomerBudgetSpent",
                table: "ProjectFinance",
                newName: "CurrentBudgetSpent");
        }
    }
}
