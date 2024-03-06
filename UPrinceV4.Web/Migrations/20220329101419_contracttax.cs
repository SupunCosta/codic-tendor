using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class contracttax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContractTaxonomy",
                columns: new[] { "Id", "ContractId", "ContractTaxonomyLevelId", "ParentId", "Title" },
                values: new object[] { "lgkf9fe-po57-4088-SANI-d27008dflfdkg", "gyjkab9fe-po57-4088-SANI-d27008688fss", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", null, "Sanitary" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "lgkf9fe-po57-4088-SANI-d27008dflfdkg");
        }
    }
}
