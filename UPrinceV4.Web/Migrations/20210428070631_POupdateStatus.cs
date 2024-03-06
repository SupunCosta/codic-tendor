using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POupdateStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "POHeader",
                newName: "POTypeId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "POHeader",
                newName: "POStatusId");

            migrationBuilder.CreateTable(
                name: "POStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "POType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "POStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-657dd897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-8723-213d2d1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "813a0c70-b58f-433d-8945-9cb166ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-ee367a82addb" }
                });

            migrationBuilder.InsertData(
                table: "POType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 1, "en", "Resources", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3", 2, "en", "Product", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae1", 1, "nl", "Resources", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac2", 2, "nl", "Product", "4010e768-3e06-4702-b337-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POStatus");

            migrationBuilder.DropTable(
                name: "POType");

            migrationBuilder.RenameColumn(
                name: "POTypeId",
                table: "POHeader",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "POStatusId",
                table: "POHeader",
                newName: "Status");
        }
    }
}
