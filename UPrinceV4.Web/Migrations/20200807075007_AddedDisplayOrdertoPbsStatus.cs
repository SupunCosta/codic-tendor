using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedDisplayOrdertoPbsStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "PbsProductStatus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "PbsProductItemType",
                columns: new[] { "Id", "LocaleCode", "Name" },
                values: new object[,]
                {
                    { "48a7dd9c-55ac-4e7c-a2f3-653811c0eb14", "PbsProductItemType.48a7dd9c-55ac-4e7c-a2f3-653811c0eb14", "External Products" },
                    { "aa0c8e3c-f716-4f92-afee-851d485164da", "PbsProductItemType.aa0c8e3c-f716-4f92-afee-851d485164da", "Internal Products" }
                });

            migrationBuilder.InsertData(
                table: "PbsProductStatus",
                columns: new[] { "Id", "DisplayOrder", "LocaleCode", "Name" },
                values: new object[,]
                {
                    { "d60aad0b-2e84-482b-ad25-618d80d49477", 1, "PbsProductStatus.d60aad0b-2e84-482b-ad25-618d80d49477", "Pending Development" },
                    { "94282458-0b40-40a3-b0f9-c2e40344c8f1", 2, "PbsProductStatus.94282458-0b40-40a3-b0f9-c2e40344c8f1", "In Development" },
                    { "7143ff01-d173-4a20-8c17-cacdfecdb84c", 3, "PbsProductStatus.7143ff01-d173-4a20-8c17-cacdfecdb84c", "In Review" },
                    { "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", 4, "PbsProductStatus.7bcb4e8d-8e8c-487d-8170-6b91c89fc3da", "Approved" },
                    { "4010e768-3e06-4702-b337-ee367a82addb", 5, "PbsProductStatus.4010e768-3e06-4702-b337-ee367a82addb", "Handed Order" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PbsProductItemType",
                keyColumn: "Id",
                keyValue: "48a7dd9c-55ac-4e7c-a2f3-653811c0eb14");

            migrationBuilder.DeleteData(
                table: "PbsProductItemType",
                keyColumn: "Id",
                keyValue: "aa0c8e3c-f716-4f92-afee-851d485164da");

            migrationBuilder.DeleteData(
                table: "PbsProductStatus",
                keyColumn: "Id",
                keyValue: "4010e768-3e06-4702-b337-ee367a82addb");

            migrationBuilder.DeleteData(
                table: "PbsProductStatus",
                keyColumn: "Id",
                keyValue: "7143ff01-d173-4a20-8c17-cacdfecdb84c");

            migrationBuilder.DeleteData(
                table: "PbsProductStatus",
                keyColumn: "Id",
                keyValue: "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da");

            migrationBuilder.DeleteData(
                table: "PbsProductStatus",
                keyColumn: "Id",
                keyValue: "94282458-0b40-40a3-b0f9-c2e40344c8f1");

            migrationBuilder.DeleteData(
                table: "PbsProductStatus",
                keyColumn: "Id",
                keyValue: "d60aad0b-2e84-482b-ad25-618d80d49477");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "PbsProductStatus");
        }
    }
}
