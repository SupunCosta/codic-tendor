using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class certificationTaxonomy1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CertificationTaxonomy",
                columns: new[] { "Id", "CertificationTaxonomyLevelId", "CetificationId", "ParentCertificationId", "ParentId", "Title" },
                values: new object[,]
                {
                    { "115ab9fe-po57-4088-82a9-d27008688bvv", "qq282458-0b40-poa3-b0f9-c2e40344c8qq", "115ab9fe-po57-4088-82a9-d27008688bvv", "115ab9fe-po57-4088-82a9-d27008688bvv", null, "Qualification" },
                    { "22d9e479-pob3-40c6-ad61-e40dbe6a51uu", "qq282458-0b40-poa3-b0f9-c2e40344c8qq", "22d9e479-pob3-40c6-ad61-e40dbe6a51uu", "22d9e479-pob3-40c6-ad61-e40dbe6a51uu", "115ab9fe-po57-4088-82a9-d27008688bvv", "Qualification Type 1" },
                    { "335ab9fe-po57-4088-82a9-d27008688tgg", "qq282458-0b40-poa3-b0f9-c2e40344c8qq", "335ab9fe-po57-4088-82a9-d27008688tgg", "335ab9fe-po57-4088-82a9-d27008688tgg", null, "Qualification X" },
                    { "44d9e479-pob3-40c6-ad61-e40dbe6a54kk", "qq282458-0b40-poa3-b0f9-c2e40344c8qq", "44d9e479-pob3-40c6-ad61-e40dbe6a54kk", "44d9e479-pob3-40c6-ad61-e40dbe6a54kk", "335ab9fe-po57-4088-82a9-d27008688tgg", "Qualifiaction Type 2" }
                });

            migrationBuilder.InsertData(
                table: "CertificationTaxonomyLevel",
                columns: new[] { "Id", "DisplayOrder", "IsChildren", "LanguageCode", "LevelId", "Name" },
                values: new object[,]
                {
                    { "vv5ab9fe-po57-4088-82a9-d27008688bvv", 1, true, "en", "qq282458-0b40-poa3-b0f9-c2e40344c8qq", "Type" },
                    { "uud9e479-pob3-40c6-ad61-e40dbe6a51uu", 2, true, "en", "2210e768-3e06-po02-b337-ee367a82ad22", "Level" },
                    { "gg5ab9fe-po57-4088-82a9-d27008688tgg", 1, true, "nl", "qq282458-0b40-poa3-b0f9-c2e40344c8qq", "Type nl" },
                    { "kkd9e479-pob3-40c6-ad61-e40dbe6a54kk", 2, true, "nl", "2210e768-3e06-po02-b337-ee367a82ad22", "Level nl" },
                    { "ttkab9fe-po57-4088-82a9-d27008688btt", 3, true, "en", "oo10e768-3e06-po02-b337-ee367a82adoo", "Certification" },
                    { "eew9e479-pob3-40c6-ad61-e40dbe6a51ee", 3, true, "nl", "oo10e768-3e06-po02-b337-ee367a82adoo", "Certification nl" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CertificationTaxonomy",
                keyColumn: "Id",
                keyValue: "115ab9fe-po57-4088-82a9-d27008688bvv");

            migrationBuilder.DeleteData(
                table: "CertificationTaxonomy",
                keyColumn: "Id",
                keyValue: "22d9e479-pob3-40c6-ad61-e40dbe6a51uu");

            migrationBuilder.DeleteData(
                table: "CertificationTaxonomy",
                keyColumn: "Id",
                keyValue: "335ab9fe-po57-4088-82a9-d27008688tgg");

            migrationBuilder.DeleteData(
                table: "CertificationTaxonomy",
                keyColumn: "Id",
                keyValue: "44d9e479-pob3-40c6-ad61-e40dbe6a54kk");

            migrationBuilder.DeleteData(
                table: "CertificationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "eew9e479-pob3-40c6-ad61-e40dbe6a51ee");

            migrationBuilder.DeleteData(
                table: "CertificationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "gg5ab9fe-po57-4088-82a9-d27008688tgg");

            migrationBuilder.DeleteData(
                table: "CertificationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "kkd9e479-pob3-40c6-ad61-e40dbe6a54kk");

            migrationBuilder.DeleteData(
                table: "CertificationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "ttkab9fe-po57-4088-82a9-d27008688btt");

            migrationBuilder.DeleteData(
                table: "CertificationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "uud9e479-pob3-40c6-ad61-e40dbe6a51uu");

            migrationBuilder.DeleteData(
                table: "CertificationTaxonomyLevel",
                keyColumn: "Id",
                keyValue: "vv5ab9fe-po57-4088-82a9-d27008688bvv");
        }
    }
}
