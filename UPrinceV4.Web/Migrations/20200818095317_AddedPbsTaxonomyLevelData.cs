using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedPbsTaxonomyLevelData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PbsTaxonomyLevel",
                columns: new[] { "Id", "Name", "Order", "Type" },
                values: new object[,]
                {
                    { "f0d64941-145a-4a8a-8619-165c965a16eb", "Product", 100, "Location" },
                    { "077845b7-79a7-4883-a02d-6094fc6d6563", "Separation", 200, "Location" },
                    { "8bb27024-ce91-4406-8e48-db08286f0b4b", "Product", 100, "Utility" },
                    { "cd8418c0-502e-4893-b387-1426a5edd3a4", "Traject Part", 200, "Utility" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "077845b7-79a7-4883-a02d-6094fc6d6563");

            migrationBuilder.DeleteData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "8bb27024-ce91-4406-8e48-db08286f0b4b");

            migrationBuilder.DeleteData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "cd8418c0-502e-4893-b387-1426a5edd3a4");

            migrationBuilder.DeleteData(
                table: "PbsTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "f0d64941-145a-4a8a-8619-165c965a16eb");
        }
    }
}
