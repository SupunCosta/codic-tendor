using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class vpFilterDropdown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VpFilterDropdown",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VpFilterDropdown", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VpFilterDropdown",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "aqwab9fe-po57-4088-82a9-d27008688mvk", 1, "Project", "en", "Project", "5" },
                    { "slaab9fe-po57-4088-82a9-d27008688kgd", 1, "Project", "nl", "Project(nl)", "5" },
                    { "qwe9e479-pob3-40c6-ad61-e40dbe6a5lks", 2, "Last Week", "en", "Last Week", "1" },
                    { "eew9e479-pob3-40c6-ad61-e40dbe6a5111", 2, "Last Week", "nl", "Last Week(nl)", "1" },
                    { "qqqab9fe-qq57-4088-82a9-d27008688qqq", 3, "Current Week", "en", "Current Week", "2" },
                    { "zzzab9fe-po57-4088-82a9-d27008688zzz", 3, "Current Week", "nl", "Current Week(nl)", "2" },
                    { "bnb9e479-pob3-40c6-ad61-e40dbe6a5bnb", 4, "Last Month", "en", "Last Month", "3" },
                    { "wer9e479-pob3-40c6-ad61-e40dbe6a5wer", 4, "Last Month", "nl", "Last Month(nl)", "3" },
                    { "jfdjjf79-pob3-40c6-ad61-e40dbehdhbfh", 5, "Current Month", "en", "Current Month", "4" },
                    { "ytisid79-pob3-40c6-ad61-e40dbejfsjjd", 5, "Current Month", "nl", "Current Month(nl)", "4" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VpFilterDropdown");
        }
    }
}
