using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class bmProjectClassificationDropdown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectClassificationBuisnessUnit",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectClassificationBuisnessUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectClassificationConstructionType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectClassificationConstructionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectClassificationSector",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectClassificationSector", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectClassificationSize",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectClassificationSize", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProjectClassificationBuisnessUnit",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", 1, "Civil", "en", "Civil", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "dgdgsa893-0267-48af-b65a-b7a93ebdfdgg", 1, "Civil", "nl", "Civil(nl)", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "fj56a893-0267-48af-b65a-b7a93ebddsndsgk", 2, "Buisness Unit 2", "en", "Buisness Unit 2", "jdsjg4e8d-fhhd-487d-8170-6b91c89fdsgksg" },
                    { "hjjgsa893-0267-48af-b65a-b7a93ebdfdmgmmm", 2, "Buisness Unit 2", "nl", "Buisness Unit 2(nl)", "jdsjg4e8d-fhhd-487d-8170-6b91c89fdsgksg" }
                });

            migrationBuilder.InsertData(
                table: "ProjectClassificationConstructionType",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "ncngsa893-0267-48af-b65a-b7a93ebdfdmgmmm", 2, "ConstructionType 2", "nl", "ConstructionType 2(nl)", "eeedkjg4e8d-fhhd-487d-8170-6b91c89fdddeee" },
                    { "ooo6a893-0267-48af-b65a-b7a93ebddsndsgk", 2, "ConstructionType 2", "en", "ConstructionType 2", "eeedkjg4e8d-fhhd-487d-8170-6b91c89fdddeee" },
                    { "qqqgsa893-0267-48af-b65a-b7a93ebdfdgg", 1, "Pontoon", "nl", "Pontoon(nl)", "zzzbk4e8d-8e8c-487d-8170-6b91c89fczzz" },
                    { "uurya893-0267-48af-b65a-b7a93ebd1wem", 1, "Pontoon", "en", "Pontoon", "zzzbk4e8d-8e8c-487d-8170-6b91c89fczzz" }
                });

            migrationBuilder.InsertData(
                table: "ProjectClassificationSector",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "kkk93-jsjj-fmms-amdm-b7a93ebd1wem", 1, "Harbour", "nl", "Harbour(nl)", "bbbbk4e8d-8e8c-487d-8170-6b91c89fcbbb" },
                    { "ppp93-jsjj-fmms-amdm-b7a93ebd1wem", 2, "Sector 2", "en", "Sector 2", "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv" },
                    { "ya893-jsjj-fmms-amdm-b7a93ebd1wem", 1, "Harbour", "en", "Harbour", "bbbbk4e8d-8e8c-487d-8170-6b91c89fcbbb" },
                    { "zzz93-jsjj-fmms-amdm-b7a93ebd1wem", 2, "Sector 2", "nl", "Sector 2(nl)", "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv" }
                });

            migrationBuilder.InsertData(
                table: "ProjectClassificationSize",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "dgdgsa893-0267-48af-b65a-b7a93ebdfdgg", 1, "1-999", "nl", "1-999(nl)", "ifnfk4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "dmagsa893-0267-48af-b65a-b7a93ebdfdmgmmm", 2, "Size 2", "nl", "Size 2(nl)", "kjsdkjg4e8d-fhhd-487d-8170-6b91c89fdddaad" },
                    { "iisia893-0267-48af-b65a-b7a93ebd1ccf", 1, "1-999", "en", "1-999", "ifnfk4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "nnn6a893-0267-48af-b65a-b7a93ebddsndsgk", 2, "Size 2", "en", "Size 2", "kjsdkjg4e8d-fhhd-487d-8170-6b91c89fdddaad" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectClassificationBuisnessUnit");

            migrationBuilder.DropTable(
                name: "ProjectClassificationConstructionType");

            migrationBuilder.DropTable(
                name: "ProjectClassificationSector");

            migrationBuilder.DropTable(
                name: "ProjectClassificationSize");
        }
    }
}
