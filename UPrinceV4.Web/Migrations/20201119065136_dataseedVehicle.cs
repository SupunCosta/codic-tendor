using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class dataseedVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "CpcResourceFamily",
               columns: new[] { "Id", "Title", "LocaleCode", "ParentId" , "DisplayOrder" },
               values: new object[] { "0c355800-91fd-4d99-8010-921a42f0ba04", "Vehicle", "Vehicle", null ,0});

            migrationBuilder.InsertData(
               table: "CpcResourceFamilyLocalizedData",
               columns: new[] { "Id", "Label", "LanguageCode", "CpcResourceFamilyId", "ParentId" },
               values: new object[] { "72d58f8f-5bcf-4415-86aa-b6677712ba47", "Vehicle", "en", "0c355800-91fd-4d99-8010-921a42f0ba04", null });

            migrationBuilder.InsertData(
               table: "CpcResourceFamilyLocalizedData",
               columns: new[] { "Id", "Label", "LanguageCode", "CpcResourceFamilyId", "ParentId" },
               values: new object[] { "b6677712ba47-5bcf-4415-86aa-72d58f8f", "Vehicle(nl)", "nl", "0c355800-91fd-4d99-8010-921a42f0ba04", null });

            migrationBuilder.InsertData(
               table: "CpcResourceFamilyLocalizedData",
               columns: new[] { "Id", "Label", "LanguageCode", "CpcResourceFamilyId", "ParentId" },
               values: new object[] { "77712ba47-4415-5bcfb66-4415-86aa-72d58f8f", "Vehicle(nl-BE)", "nl-BE", "0c355800-91fd-4d99-8010-921a42f0ba04", null });

            migrationBuilder.InsertData(
              table: "CpcBasicUnitOfMeasure",
              columns: new[] { "Id", "Name", "DisplayOrder", "LocaleCode" },
              values: new object[] { "0c355800-91fd-4d99-8010-921a42f0ba04", "km", 0, "km" });

            migrationBuilder.InsertData(
              table: "CpcBasicUnitOfMeasureLocalizedData",
              columns: new[] { "Id", "Label", "LanguageCode", "CpcBasicUnitOfMeasureId" },
              values: new object[] { "582f2318-287f-45b6-b628-20183f3ccfe4", "km", "en", "0c355800-91fd-4d99-8010-921a42f0ba04"});

            migrationBuilder.InsertData(
             table: "CpcBasicUnitOfMeasureLocalizedData",
             columns: new[] { "Id", "Label", "LanguageCode", "CpcBasicUnitOfMeasureId" },
             values: new object[] { "20183f3ccfe4-287f-45b6-b628-582f2318", "km", "nl", "0c355800-91fd-4d99-8010-921a42f0ba04" });

        }
    }
}
