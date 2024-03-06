using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class seedgoddeeris : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "2274a893-0267-48af-b65a-b7a93ebddsn785564");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "7777sa893-0267-48af-b65a-b7a93ebdfdm7788");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "fj56a893-0267-48af-b65a-b7a93ebddsndsgk");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "hjjgsa893-0267-48af-b65a-b7a93ebdfdmgmmm");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "nmbba893-0267-48af-b65a-b7a93ebdfd87945");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "wsdba893-0267-48af-b65a-b7a93ebddsn7lkjh");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "OrganizationTaxonomy");

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Goddeeris", "Goddeeris" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "dgdgsa893-0267-48af-b65a-b7a93ebdfdgg",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Goddeeris", "Goddeeris" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "OrganizationTaxonomy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Stability", "Stability" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "dgdgsa893-0267-48af-b65a-b7a93ebdfdgg",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Stability", "Stabiliteit" });

            migrationBuilder.InsertData(
                table: "ProjectClassificationBuisnessUnit",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "2274a893-0267-48af-b65a-b7a93ebddsn785564", 3, "BIM", "en", "BIM", "222jg4e8d-fhhd-487d-8170-6b91c89fdsg222" },
                    { "7777sa893-0267-48af-b65a-b7a93ebdfdm7788", 4, "Civil Engineering", "nl", "Civil Engineering", "5555g4e8d-fhhd-487d-8170-6b91c89fds5555" },
                    { "fj56a893-0267-48af-b65a-b7a93ebddsndsgk", 2, "Special Techniques", "en", "Special Techniques", "jdsjg4e8d-fhhd-487d-8170-6b91c89fdsgksg" },
                    { "hjjgsa893-0267-48af-b65a-b7a93ebdfdmgmmm", 2, "Special Techniques", "nl", "Speciale technieken", "jdsjg4e8d-fhhd-487d-8170-6b91c89fdsgksg" },
                    { "nmbba893-0267-48af-b65a-b7a93ebdfd87945", 3, "BIM", "nl", "BIM", "222jg4e8d-fhhd-487d-8170-6b91c89fdsg222" },
                    { "wsdba893-0267-48af-b65a-b7a93ebddsn7lkjh", 4, "Civil Engineering", "en", "Civil Engineering", "5555g4e8d-fhhd-487d-8170-6b91c89fds5555" }
                });
        }
    }
}
