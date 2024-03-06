using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class bmContractorTaxonomy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractTaxonomyLevelId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTaxonomy", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ContractTaxonomy",
                columns: new[] { "Id", "ContractId", "ContractTaxonomyLevelId", "ParentId", "Title" },
                values: new object[,]
                {
                    { "dgb9fe-po57-4088-82a9-d27008dfgjujj", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "hjkdab9fe-po57-4088-82a9-d27008688dhjh", "Contractor 111 " },
                    { "fhgjj9fe-po57-4088-82a9-d27008dfgfdgdf", "dgdgsb9fe-afffg-dfg-sdd-d2700868uhsfs", "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf", "gdab9fe-po57-4088-82a9-d27008dfghhj", "Contractor 333 " },
                    { "gdab9fe-po57-4088-82a9-d27008dfghhj", "gyjkab9fe-po57-4088-82a9-d27008688fss", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", null, "Contractor Y " },
                    { "ghjkkjj9fe-po57-4088-82a9-d27008dfgfsdsd", "dgvdgsb9fe-afffg-dfg-sdd-d2700868ufgs", "fdsf282458-0b40-poa3-b0f9-c2e40344c8dsdff", "fhgjj9fe-po57-4088-82a9-d27008dfgfdgdf", "Contractor 444 " },
                    { "hjkdab9fe-po57-4088-82a9-d27008688dhjh", "335ab9fe-po57-4088-82a9-d27008688tgg", "qq282458-0b40-poa3-b0f9-c2e40344c8qq", null, "Contractor X" },
                    { "jgjdj9fe-po57-4088-82a9-d27008dfgsdfa", "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs", "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf", "dgb9fe-po57-4088-82a9-d27008dfgjujj", "Contractor 222 " }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractTaxonomy");
        }
    }
}
