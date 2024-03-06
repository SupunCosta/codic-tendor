using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class absenceTypeSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CpcLabourItemId",
                table: "HRHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "AbsenceLeaveType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AbsenceLeaveType",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "aqwab9fe-po57-4088-82a9-d27008688mvk", 1, "Casual", "en", "Casual", "lkkab9fe-po57-4088-82a9-d27008688lkk" },
                    { "slaab9fe-po57-4088-82a9-d27008688kgd", 1, "Casual", "nl", "Casual(nl)", "lkkab9fe-po57-4088-82a9-d27008688lkk" },
                    { "qwe9e479-pob3-40c6-ad61-e40dbe6a5lks", 2, "Annual", "en", "Annual", "fg10e768-3e06-po02-b337-ee367a82afff" },
                    { "eew9e479-pob3-40c6-ad61-e40dbe6a5111", 2, "Annual", "nl", "Annual(nl)", "fg10e768-3e06-po02-b337-ee367a82afff" },
                    { "qqqab9fe-qq57-4088-82a9-d27008688qqq", 3, "Medical", "en", "Medical", "oo10e768-3e06-po02-b337-ee367a82adooo" },
                    { "zzzab9fe-po57-4088-82a9-d27008688zzz", 3, "Medical", "nl", "Medical(nl)", "oo10e768-3e06-po02-b337-ee367a82adooo" },
                    { "bnb9e479-pob3-40c6-ad61-e40dbe6a5bnb", 4, "Maternity", "en", "Maternity", "2210e768-3e06-po02-b337-ee367a82ad22" },
                    { "wer9e479-pob3-40c6-ad61-e40dbe6a5wer", 4, "Maternity", "nl", "Maternity(nl)", "2210e768-3e06-po02-b337-ee367a82ad22" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AbsenceLeaveType",
                keyColumn: "Id",
                keyValue: "aqwab9fe-po57-4088-82a9-d27008688mvk");

            migrationBuilder.DeleteData(
                table: "AbsenceLeaveType",
                keyColumn: "Id",
                keyValue: "bnb9e479-pob3-40c6-ad61-e40dbe6a5bnb");

            migrationBuilder.DeleteData(
                table: "AbsenceLeaveType",
                keyColumn: "Id",
                keyValue: "eew9e479-pob3-40c6-ad61-e40dbe6a5111");

            migrationBuilder.DeleteData(
                table: "AbsenceLeaveType",
                keyColumn: "Id",
                keyValue: "qqqab9fe-qq57-4088-82a9-d27008688qqq");

            migrationBuilder.DeleteData(
                table: "AbsenceLeaveType",
                keyColumn: "Id",
                keyValue: "qwe9e479-pob3-40c6-ad61-e40dbe6a5lks");

            migrationBuilder.DeleteData(
                table: "AbsenceLeaveType",
                keyColumn: "Id",
                keyValue: "slaab9fe-po57-4088-82a9-d27008688kgd");

            migrationBuilder.DeleteData(
                table: "AbsenceLeaveType",
                keyColumn: "Id",
                keyValue: "wer9e479-pob3-40c6-ad61-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "AbsenceLeaveType",
                keyColumn: "Id",
                keyValue: "zzzab9fe-po57-4088-82a9-d27008688zzz");

            migrationBuilder.DropColumn(
                name: "CpcLabourItemId",
                table: "HRHeader");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "AbsenceLeaveType");
        }
    }
}
