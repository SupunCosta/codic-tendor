using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class mstype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "MilestoneStatus",
                newName: "StatusId");

            migrationBuilder.InsertData(
                table: "MilestoneType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "aqwab9fe-msms-4088-82a9-d27008688mvk", 1, "en", "Scope : Product States", "lkkab9fe-po57-4088-82a9-d27008688lkk" },
                    { "slaab9fe-msms-4088-82a9-d27008688kgd", 1, "nl", "Scope : Product States(nl)", "lkkab9fe-po57-4088-82a9-d27008688lkk" },
                    { "qwe9e479-msms-40c6-ad61-e40dbe6a5lks", 2, "en", "Finance : invoices and payments", "fg10e768-3e06-po02-b337-ee367a82afff" },
                    { "eew9e479-msms-40c6-ad61-e40dbe6a5111", 2, "nl", "Finance : invoices and payments(nl)", "fg10e768-3e06-po02-b337-ee367a82afff" },
                    { "qqqab9fe-msms-4088-82a9-d27008688qqq", 3, "en", "Quality Activities", "oo10e768-3e06-po02-b337-ee367a82adooo" },
                    { "zzzab9fe-msms-4088-82a9-d27008688zzz", 3, "nl", "Quality Activities(nl)", "oo10e768-3e06-po02-b337-ee367a82adooo" },
                    { "bnb9e479-msms-40c6-ad61-e40dbe6a5bnb", 4, "en", "Risk Assessments", "2210e768-3e06-po02-b337-ee367a82ad22" },
                    { "wer9e479-msms-40c6-ad61-e40dbe6a5wer", 4, "nl", "Risk Assessments(nl)", "2210e768-3e06-po02-b337-ee367a82ad22" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MilestoneType",
                keyColumn: "Id",
                keyValue: "aqwab9fe-msms-4088-82a9-d27008688mvk");

            migrationBuilder.DeleteData(
                table: "MilestoneType",
                keyColumn: "Id",
                keyValue: "bnb9e479-msms-40c6-ad61-e40dbe6a5bnb");

            migrationBuilder.DeleteData(
                table: "MilestoneType",
                keyColumn: "Id",
                keyValue: "eew9e479-msms-40c6-ad61-e40dbe6a5111");

            migrationBuilder.DeleteData(
                table: "MilestoneType",
                keyColumn: "Id",
                keyValue: "qqqab9fe-msms-4088-82a9-d27008688qqq");

            migrationBuilder.DeleteData(
                table: "MilestoneType",
                keyColumn: "Id",
                keyValue: "qwe9e479-msms-40c6-ad61-e40dbe6a5lks");

            migrationBuilder.DeleteData(
                table: "MilestoneType",
                keyColumn: "Id",
                keyValue: "slaab9fe-msms-4088-82a9-d27008688kgd");

            migrationBuilder.DeleteData(
                table: "MilestoneType",
                keyColumn: "Id",
                keyValue: "wer9e479-msms-40c6-ad61-e40dbe6a5wer");

            migrationBuilder.DeleteData(
                table: "MilestoneType",
                keyColumn: "Id",
                keyValue: "zzzab9fe-msms-4088-82a9-d27008688zzz");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "MilestoneStatus",
                newName: "TypeId");
        }
    }
}
