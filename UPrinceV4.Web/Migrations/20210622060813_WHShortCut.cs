using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class WHShortCut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockShortCutPane",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockShortCutPane", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WFShortCutPane",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WFShortCutPane", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WHShortCutPane",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WHShortCutPane", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "StockShortCutPane",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name" },
                values: new object[,]
                {
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac1", 1, "en", "All" },
                    { "bdd9e478-75b3-40c6-ad61-e40dbe6a51ac2", 2, "en", "Material" },
                    { "bdd9e477-75b3-40c6-ad61-e40dbe6a51ac3", 3, "en", "Tool" },
                    { "bdd9e476-75b3-40c6-ad61-e40dbe6a51ac4", 4, "en", "Consumables" },
                    { "bdd9e475-75b3-40c6-ad61-e40dbe6a51ac5", 1, "nl", "All" },
                    { "bdd9e474-75b3-40c6-ad61-e40dbe6a51ac6", 2, "nl", "Material" },
                    { "bdd9e473-75b3-40c6-ad61-e40dbe6a51ac7", 3, "nl", "Tool" },
                    { "bdd9e472-75b3-40c6-ad61-e40dbe6a51ac8", 4, "nl", "Consumables" }
                });

            migrationBuilder.InsertData(
                table: "WFShortCutPane",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-82a9-d27008688bae6", 3, "nl", "Done" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac5", 2, "nl", "To Be Assigned" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae4", 1, "nl", "All" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2", 2, "en", "To Be Assigned" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae1", 1, "en", "All" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae3", 3, "en", "Done" }
                });

            migrationBuilder.InsertData(
                table: "WHShortCutPane",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name" },
                values: new object[,]
                {
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac15", 2, "nl", "Last" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae11", 1, "en", "All" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac12", 2, "en", "Last" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae13", 3, "en", "Tree View" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae14", 1, "nl", "All" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae16", 3, "nl", "Tree View" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockShortCutPane");

            migrationBuilder.DropTable(
                name: "WFShortCutPane");

            migrationBuilder.DropTable(
                name: "WHShortCutPane");
        }
    }
}
