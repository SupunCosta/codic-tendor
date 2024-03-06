using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class contractorTaxonomySeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "ghjkkjj9fe-po57-4088-82a9-d27008dfgfsdsd");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "jgjdj9fe-po57-4088-82a9-d27008dfgsdfa");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "ProjectTeamRole",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "dgb9fe-po57-4088-82a9-d27008dfgjujj",
                column: "Title",
                value: "All -Air Systems");

            migrationBuilder.UpdateData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "fhgjj9fe-po57-4088-82a9-d27008dfgfdgdf",
                columns: new[] { "ParentId", "Title" },
                values: new object[] { "lgkf9fe-po57-4088-82a9-d27008dflfdkg", "Wet Pipe System" });

            migrationBuilder.UpdateData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "gdab9fe-po57-4088-82a9-d27008dfghhj",
                column: "Title",
                value: "Electricity");

            migrationBuilder.UpdateData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "hjkdab9fe-po57-4088-82a9-d27008688dhjh",
                column: "Title",
                value: "HVAC");

            migrationBuilder.InsertData(
                table: "ContractTaxonomy",
                columns: new[] { "Id", "ContractId", "ContractTaxonomyLevelId", "ParentId", "Title" },
                values: new object[,]
                {
                    { "5569fe-po57-4088-82a9-d27008dfgj3ien", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "hjkdab9fe-po57-4088-82a9-d27008688dhjh", "Air-Water Systems" },
                    { "bfer9fe-afmd-lsdd-kafd-d27008dfgjaasgkv", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "dgb9fe-po57-4088-82a9-d27008dfgjujj", "Variable Air Volume(VAV)" },
                    { "bfjs9fe-afmd-lsdd-kafd-d27008dfgjaadid", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "dgb9fe-po57-4088-82a9-d27008dfgjujj", "Terminal Reheat" },
                    { "jfgf9fe-po57-4088-82a9-d27008dfgjabktyk", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "hjkdab9fe-po57-4088-82a9-d27008688dhjh", "Heating and Cooling panels" },
                    { "kdca9fe-afmd-lsdd-kafd-d27008dfgjaawfm", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "dgb9fe-po57-4088-82a9-d27008dfgjujj", "Dual duct" },
                    { "kjhgj9fe-po57-4088-82a9-d27008dfgrytut", "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs", "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf", "gdab9fe-po57-4088-82a9-d27008dfghhj", "High Voltage" },
                    { "lgkf9fe-po57-4088-82a9-d27008dflfdkg", "gyjkab9fe-po57-4088-82a9-d27008688fss", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", null, "Sprinklers" },
                    { "lgkf9fe-po57-4088-82a9-d27008dtyjh", "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs", "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf", "mbgej9fe-po57-4088-82a9-d27008dfgrthg", "Low Voltage Connection" },
                    { "lj989fe-afmd-lsdd-kafd-d27008dfgjksfn", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "dgb9fe-po57-4088-82a9-d27008dfgjujj", "Single Zone" },
                    { "lkdfkjj9fe-po57-4088-82a9-d27008dfgfsdfkk", "dgvdgsb9fe-afffg-dfg-sdd-d2700868ufgs", "fdsf282458-0b40-poa3-b0f9-c2e40344c8dsdff", "lgkf9fe-po57-4088-82a9-d27008dflfdkg", "Pre-Action System" },
                    { "lmdfkfe-po57-4088-82a9-d27008dfgasdk", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "5569fe-po57-4088-82a9-d27008dfgj3ien", "Fan Coil Units" },
                    { "mbgej9fe-po57-4088-82a9-d27008dfgrthg", "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs", "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf", "gdab9fe-po57-4088-82a9-d27008dfghhj", "Lightning & Power" },
                    { "mcd9fe-po57-4088-82a9-d27008dfgjabfad", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "hjkdab9fe-po57-4088-82a9-d27008688dhjh", "All-Water Systems" },
                    { "mkngj9fe-po57-4088-82a9-d27008dfgrwwff", "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs", "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf", "gdab9fe-po57-4088-82a9-d27008dfghhj", "Low Power Installation" },
                    { "mnnckfe-po57-4088-82a9-d27008dfgksck", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "5569fe-po57-4088-82a9-d27008dfgj3ien", "Induction Units" },
                    { "mvffkjj9fe-po57-4088-82a9-d27008dfgfsdmmd", "dgvdgsb9fe-afffg-dfg-sdd-d2700868ufgs", "fdsf282458-0b40-poa3-b0f9-c2e40344c8dsdff", "lgkf9fe-po57-4088-82a9-d27008dflfdkg", "Deluge System" },
                    { "mvnd9fe-afmd-lsdd-kafd-d27008dfgjajfnd", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "dgb9fe-po57-4088-82a9-d27008dfgjujj", "MultiZone" },
                    { "qcfj9fe-po57-4088-82a9-d27008dfoiy", "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs", "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf", "mbgej9fe-po57-4088-82a9-d27008dfgrthg", "Low Voltage Ground" },
                    { "qertfe-po57-4088-82a9-d27008dfgjabqert", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "mcd9fe-po57-4088-82a9-d27008dfgjabfad", "Fan Coil Units" },
                    { "ujgkkjj9fe-po57-4088-82a9-d27008dfgfsdmkf", "dgvdgsb9fe-afffg-dfg-sdd-d2700868ufgs", "fdsf282458-0b40-poa3-b0f9-c2e40344c8dsdff", "lgkf9fe-po57-4088-82a9-d27008dflfdkg", "Dry Pipe System" },
                    { "yiuo9fe-po57-4088-82a9-d27008dfgjabksvn", "jgjkab9fe-po57-4088-82a9-d2700868uhfhf", "fdh282458-0b40-poa3-b0f9-c2e40344c8fgh", "hjkdab9fe-po57-4088-82a9-d27008688dhjh", "Water-source Heat Pumps" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "5569fe-po57-4088-82a9-d27008dfgj3ien");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "bfer9fe-afmd-lsdd-kafd-d27008dfgjaasgkv");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "bfjs9fe-afmd-lsdd-kafd-d27008dfgjaadid");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "jfgf9fe-po57-4088-82a9-d27008dfgjabktyk");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "kdca9fe-afmd-lsdd-kafd-d27008dfgjaawfm");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "kjhgj9fe-po57-4088-82a9-d27008dfgrytut");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "lgkf9fe-po57-4088-82a9-d27008dflfdkg");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "lgkf9fe-po57-4088-82a9-d27008dtyjh");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "lj989fe-afmd-lsdd-kafd-d27008dfgjksfn");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "lkdfkjj9fe-po57-4088-82a9-d27008dfgfsdfkk");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "lmdfkfe-po57-4088-82a9-d27008dfgasdk");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "mbgej9fe-po57-4088-82a9-d27008dfgrthg");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "mcd9fe-po57-4088-82a9-d27008dfgjabfad");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "mkngj9fe-po57-4088-82a9-d27008dfgrwwff");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "mnnckfe-po57-4088-82a9-d27008dfgksck");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "mvffkjj9fe-po57-4088-82a9-d27008dfgfsdmmd");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "mvnd9fe-afmd-lsdd-kafd-d27008dfgjajfnd");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "qcfj9fe-po57-4088-82a9-d27008dfoiy");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "qertfe-po57-4088-82a9-d27008dfgjabqert");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "ujgkkjj9fe-po57-4088-82a9-d27008dfgfsdmkf");

            migrationBuilder.DeleteData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "yiuo9fe-po57-4088-82a9-d27008dfgjabksvn");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "ProjectTeamRole");

            migrationBuilder.UpdateData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "dgb9fe-po57-4088-82a9-d27008dfgjujj",
                column: "Title",
                value: "Contractor 111 ");

            migrationBuilder.UpdateData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "fhgjj9fe-po57-4088-82a9-d27008dfgfdgdf",
                columns: new[] { "ParentId", "Title" },
                values: new object[] { "gdab9fe-po57-4088-82a9-d27008dfghhj", "Contractor 333 " });

            migrationBuilder.UpdateData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "gdab9fe-po57-4088-82a9-d27008dfghhj",
                column: "Title",
                value: "Contractor Y ");

            migrationBuilder.UpdateData(
                table: "ContractTaxonomy",
                keyColumn: "Id",
                keyValue: "hjkdab9fe-po57-4088-82a9-d27008688dhjh",
                column: "Title",
                value: "Contractor X");

            migrationBuilder.InsertData(
                table: "ContractTaxonomy",
                columns: new[] { "Id", "ContractId", "ContractTaxonomyLevelId", "ParentId", "Title" },
                values: new object[,]
                {
                    { "ghjkkjj9fe-po57-4088-82a9-d27008dfgfsdsd", "dgvdgsb9fe-afffg-dfg-sdd-d2700868ufgs", "fdsf282458-0b40-poa3-b0f9-c2e40344c8dsdff", "fhgjj9fe-po57-4088-82a9-d27008dfgfdgdf", "Contractor 444 " },
                    { "jgjdj9fe-po57-4088-82a9-d27008dfgsdfa", "jfgsab9fe-afffg-dfg-sdd-d2700868uhsfs", "frgh282458-0b40-poa3-b0f9-c2e40344c8dsaf", "dgb9fe-po57-4088-82a9-d27008dfgjujj", "Contractor 222 " }
                });
        }
    }
}
