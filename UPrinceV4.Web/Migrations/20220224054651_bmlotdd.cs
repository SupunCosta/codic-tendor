using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class bmlotdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BMLotFileType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotFileType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotStatus",
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
                    table.PrimaryKey("PK_BMLotStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BMLotFileType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "bnb9e479-msms-40c6-BMLot-e40dbe6a5bnb", 3, "en", "URL", "2210e768-msms-po02-BMLot-ee367a82ad22" },
                    { "qqqab9fe-msms-4088-BMLot-d27008688qqq", 1, "en", "pdf", "oo10e768-msms-po02-BMLot-ee367a82adooo" },
                    { "wer9e479-msms-40c6-BMLot-e40dbe6a5wer", 4, "en", "Word Document", "2210e768-msms-po02-BMLot-ee367a82ad22" },
                    { "zzzab9fe-msms-4088-BMLot-d27008688zzz", 2, "en", "Image", "oo10e768-msms-po02-BMLot-ee367a82adooo" }
                });

            migrationBuilder.InsertData(
                table: "BMLotStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-BMLot897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-ad25-BMLot0d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-BMLotf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-b0f9-BMLot344c8f1" },
                    { "2732cd5a-0941-4c56-9c13-BMLota2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-ad25-BMLot0d49477" },
                    { "4e01a893-0267-48af-b65a-BMLotebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-BMLot89fc3da" },
                    { "5015743d-a2e6-4531-808d-BMLot00e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-BMLotecdb84c" },
                    { "77143c60-ff45-4ca2-8723-BMLotd1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-8170-BMLot89fc3da" },
                    { "813a0c70-b58f-433d-8945-BMLot6ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-BMLotecdb84c" },
                    { "8d109134-ee8d-4c7b-84c5-BMLot1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-b337-BMLota82addb" },
                    { "a35ab9fe-df57-4088-82a9-BMLot8688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-BMLot344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-BMLote6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-BMLota82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BMLotFileType");

            migrationBuilder.DropTable(
                name: "BMLotStatus");
        }
    }
}
