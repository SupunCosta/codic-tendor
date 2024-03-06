using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ProjectScopeStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectFinancialStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFinancialStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectScopeStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectScopeStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProjectFinancialStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 1, "en", "Invoiced", null },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3", 2, "en", "Paid", null },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae1", 1, "nl", "Gefactureerd", null },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2", 2, "nl", "Betaald", null }
                });

            migrationBuilder.InsertData(
                table: "ProjectScopeStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-657dd897f5bb", 1, "en", "In Preparation", null },
                    { "098cf409-7cb8-4076-00nl-657dd897f5bb", 1, "nl", "In Voorbereiding", null },
                    { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", 2, "en", "In Execution", null },
                    { "12e2d6c5-1ada-4e74-00nl-ce7fbf10e27c", 2, "nl", "In Uitvoering", null },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab2en", 3, "en", "In Pre Commisioning", null },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab2nl", 3, "nl", "In voorlopige bedrijfstelling", null },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1cen", 4, "en", "In Commisioning", null },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1cnl", 4, "nl", "In bedrijfstelling", null },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab276", 5, "en", "Provisional Accepted", null },
                    { "2732cd5a-0941-4c56-00nl-f5fdca2ab276", 5, "nl", "Voorlopige opgeleverd", null },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", 6, "en", "Accepted", null },
                    { "4e01a893-0267-48af-00nl-b7a93ebd1ccf", 6, "nl", "Opgeleverd", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFinancialStatus");

            migrationBuilder.DropTable(
                name: "ProjectScopeStatus");
        }
    }
}
