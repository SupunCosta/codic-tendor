using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class LOTsatusNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Stability", "Stability" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "dgdgsa893-0267-48af-b65a-b7a93ebdfdgg",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Stability", "Stabiliteit" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "fj56a893-0267-48af-b65a-b7a93ebddsndsgk",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Special Techniques", "Special Techniques" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "hjjgsa893-0267-48af-b65a-b7a93ebdfdmgmmm",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Special Techniques", "Speciale technieken" });

            migrationBuilder.InsertData(
                table: "ProjectClassificationBuisnessUnit",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "2274a893-0267-48af-b65a-b7a93ebddsn785564", 3, "BIM", "en", "BIM", "222jg4e8d-fhhd-487d-8170-6b91c89fdsg222" },
                    { "7777sa893-0267-48af-b65a-b7a93ebdfdm7788", 4, "Civil Engineering", "nl", "Civil Engineering", "5555g4e8d-fhhd-487d-8170-6b91c89fds5555" },
                    { "nmbba893-0267-48af-b65a-b7a93ebdfd87945", 3, "BIM", "nl", "BIM", "222jg4e8d-fhhd-487d-8170-6b91c89fdsg222" },
                    { "wsdba893-0267-48af-b65a-b7a93ebddsn7lkjh", 4, "Civil Engineering", "en", "Civil Engineering", "5555g4e8d-fhhd-487d-8170-6b91c89fds5555" }
                });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "ncngsa893-0267-48af-b65a-b7a93ebdfdmgmmm",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Production", "Productie" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "ooo6a893-0267-48af-b65a-b7a93ebddsndsgk",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Production", "Production" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "qqqgsa893-0267-48af-b65a-b7a93ebdfdgg",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Warehouse", "Magazijn" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "uurya893-0267-48af-b65a-b7a93ebd1wem",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Warehouse", "Warehouse" });

            migrationBuilder.InsertData(
                table: "ProjectClassificationConstructionType",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "001d99d9-1dc2-4735-92ae-c23bd131d095", 3, "Office", "en", "Office", "54302053-1edd-43f8-a203-e52285a52a16" },
                    { "00266462-f21a-43d5-ba8a-5c755a33339a", 3, "Office", "nl", "Bureaus", "54302053-1edd-43f8-a203-e52285a52a16" },
                    { "11166462-f21a-43d5-ba8a-5c755a333111", 4, "Appartments", "nl", "Appartementen", "bbb02053-1edd-43f8-a203-e52285a52bbb" },
                    { "888d99d9-1dc2-4735-92ae-c23bd131d999", 4, "Appartments", "en", "Appartments", "bbb02053-1edd-43f8-a203-e52285a52bbb" }
                });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "ppp93-jsjj-fmms-amdm-b7a93ebd1wem",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Office & Commercial", "Office & Commercial" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "zzz93-jsjj-fmms-amdm-b7a93ebd1wem",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Office & Commercial", "Office & Commercial(nl)" });

            migrationBuilder.InsertData(
                table: "ProjectClassificationSector",
                columns: new[] { "Id", "DisplayOrder", "Label", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "134669a5-ca20-4d33-ab17-827bc0011e0f", 5, "Care & education", "nl", "Care & education(nl)", "1bf365f3-072a-412e-b424-635452b4bd59" },
                    { "2d56177d-cf1f-4b56-9565-c641eed3be52", 6, "Marine & Civil works", "nl", "Marine & Civil works(nl)", "398ae077-ea6e-4619-9a14-dfa47e12aa39" },
                    { "cddca425-a124-4935-b1f3-c8db6b325058", 4, "Industry", "en", "Industry", "f73b0526-3271-49a1-a924-84861f96b5d9" },
                    { "d21f11f8-cf12-46ca-85c8-804faf7e70da", 3, "Residential", "en", "Residential", "c2ab9d4e-c4ca-4b99-8bf7-38597f6160f1" },
                    { "d4f6116f-5026-424b-94c3-e404d39db195", 3, "Residential", "nl", "Residential(nl)", "c2ab9d4e-c4ca-4b99-8bf7-38597f6160f1" },
                    { "e59146f9-8be1-4cac-81b0-076e5fd5bfa0", 4, "Industry", "nl", "Industry(nl)", "f73b0526-3271-49a1-a924-84861f96b5d9" },
                    { "qqqq73f7-8cf7-4601-9b15-d4ce46022a78", 6, "Marine & Civil works", "en", "Marine & Civil works", "398ae077-ea6e-4619-9a14-dfa47e12aa39" },
                    { "yyyy012b-e80e-4947-b5b7-9f979a53bca4", 5, "Care & education", "en", "Care & education", "1bf365f3-072a-412e-b424-635452b4bd59" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "2274a893-0267-48af-b65a-b7a93ebddsn785564");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "7777sa893-0267-48af-b65a-b7a93ebdfdm7788");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "nmbba893-0267-48af-b65a-b7a93ebdfd87945");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "wsdba893-0267-48af-b65a-b7a93ebddsn7lkjh");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "001d99d9-1dc2-4735-92ae-c23bd131d095");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "00266462-f21a-43d5-ba8a-5c755a33339a");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "11166462-f21a-43d5-ba8a-5c755a333111");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "888d99d9-1dc2-4735-92ae-c23bd131d999");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "134669a5-ca20-4d33-ab17-827bc0011e0f");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "2d56177d-cf1f-4b56-9565-c641eed3be52");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "cddca425-a124-4935-b1f3-c8db6b325058");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "d21f11f8-cf12-46ca-85c8-804faf7e70da");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "d4f6116f-5026-424b-94c3-e404d39db195");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "e59146f9-8be1-4cac-81b0-076e5fd5bfa0");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "qqqq73f7-8cf7-4601-9b15-d4ce46022a78");

            migrationBuilder.DeleteData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "yyyy012b-e80e-4947-b5b7-9f979a53bca4");

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Civil", "Civil" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "dgdgsa893-0267-48af-b65a-b7a93ebdfdgg",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Civil", "Civil(nl)" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "fj56a893-0267-48af-b65a-b7a93ebddsndsgk",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Buisness Unit 2", "Buisness Unit 2" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationBuisnessUnit",
                keyColumn: "Id",
                keyValue: "hjjgsa893-0267-48af-b65a-b7a93ebdfdmgmmm",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Buisness Unit 2", "Buisness Unit 2(nl)" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "ncngsa893-0267-48af-b65a-b7a93ebdfdmgmmm",
                columns: new[] { "Label", "Name" },
                values: new object[] { "ConstructionType 2", "ConstructionType 2(nl)" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "ooo6a893-0267-48af-b65a-b7a93ebddsndsgk",
                columns: new[] { "Label", "Name" },
                values: new object[] { "ConstructionType 2", "ConstructionType 2" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "qqqgsa893-0267-48af-b65a-b7a93ebdfdgg",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Pontoon", "Pontoon(nl)" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationConstructionType",
                keyColumn: "Id",
                keyValue: "uurya893-0267-48af-b65a-b7a93ebd1wem",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Pontoon", "Pontoon" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "ppp93-jsjj-fmms-amdm-b7a93ebd1wem",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Sector 2", "Sector 2" });

            migrationBuilder.UpdateData(
                table: "ProjectClassificationSector",
                keyColumn: "Id",
                keyValue: "zzz93-jsjj-fmms-amdm-b7a93ebd1wem",
                columns: new[] { "Label", "Name" },
                values: new object[] { "Sector 2", "Sector 2(nl)" });
        }
    }
}
