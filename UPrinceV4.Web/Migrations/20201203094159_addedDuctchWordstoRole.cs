using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedDuctchWordstoRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "LanguageCode", "RoleId", "RoleName", "TenantId" },
                values: new object[,]
                {
                    { "18d10b25-57ea-4f4e-9e76-56df09554a95", "nl", "5e622d-4783-95e6-4092004eb5e-aff848e", "Lasser", 1 },
                    { "be432c54-5860-4f56-832d-ceb3435d8b7e", "nl", "0e06111a-a513-45e0-a431-170dbd4b0d82", "Administrator", 1 },
                    { "476127cb-62db-4af7-ac8e-d4a722f8e142", "nl", "266a5f47-3489-484b-8dae-e4468c5329dn3", "Project Manager", 1 },
                    { "245d23fe-4864-4cc5-b53b-0a3b3843f0e1", "nl", "4837043c-119c-47e1-bbf2-1f32557fdf30", "Gebruiker", 1 },
                    { "1666e217-2b80-4acd-b48b-b041fe263fb9", "nl", "6f56c794-7f88-48a7-9aba-a3f95f940be4", "Project Eigenaar", 1 },
                    { "3178903c-bf36-40f7-b870-724e238684ff", "nl", "f9a0cee5-f09a-44a5-93e8-d78f84bbcbf3", "Project Ingenieur", 1 },
                    { "907b7af0-b132-4951-a2dc-6ab82d4cd40d", "en", "907b7af0-b132-4951-a2dc-6ab82d4cd40d", "Customer", 1 },
                    { "78b84ad9-6757-405a-9729-5d2af8615e07", "nl", "907b7af0-b132-4951-a2dc-6ab82d4cd40d", "Klant", 1 }
                });

            migrationBuilder.Sql("update PbsToleranceStateLocalizedData set Label ='Haalbaar' where Label ='Binnen Tolerantie'");
            migrationBuilder.Sql("update PbsToleranceStateLocalizedData set Label = 'Nog net haalbaar' where Label = 'Tolerantiegrens'");
            migrationBuilder.Sql("update PbsToleranceStateLocalizedData set Label = 'Niet meer Haalbaar' where Label = 'Buiten de tolerantiegrens'");
            migrationBuilder.Sql("update ProjectType set Name = 'Quotation' where Name = 'Capex'");
            migrationBuilder.Sql("update ProjectType set Name = 'Time and Material' where Name = 'Opex'");
            migrationBuilder.Sql("update ProjectTypeLocalizedData set Label = 'Quotation' where Label = 'Capex'");
            migrationBuilder.Sql("update ProjectTypeLocalizedData set Label = 'Time and Material' where Label = 'Opex'");
            migrationBuilder.Sql("update ProjectTypeLocalizedData set Label = 'Offerte' where Id = 'e0abfb36-f17f-49d0-b366-54de633cdc78'");
            migrationBuilder.Sql("update ProjectTypeLocalizedData set Label = 'Offerte' where Id = 'fd85ab31-f284-43de-a182-1b6844eba8b6'");
            migrationBuilder.Sql("update ProjectTypeLocalizedData set Label = 'Regie' where Id = '93f5861f-d615-4057-99a4-311cc0dd51cc'");
            migrationBuilder.Sql("update ProjectTypeLocalizedData set Label = 'Regie' where Id = 'c5d4d550-0d8c-4319-a585-04a20efe2b3c'");
            migrationBuilder.Sql("update ProjectType set Name = 'Offerte' where Id = '78557eb0-f2cf-4362-bc16-18dd4d2a51df'");
            migrationBuilder.Sql("update ProjectType set Name = 'Offerte' where Id = 'b82f4281-9ccc-48d1-8e49-af602d216ee9'");
            migrationBuilder.Sql("update ProjectType set Name = 'Regie' where Id = '4c7cdf47-6171-484c-8259-9049842d2303'");
            migrationBuilder.Sql("update ProjectType set Name = 'Regie' where Id = 'be865a74-e228-430d-8cb4-3cd7a7636ad9'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "1666e217-2b80-4acd-b48b-b041fe263fb9");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "18d10b25-57ea-4f4e-9e76-56df09554a95");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "245d23fe-4864-4cc5-b53b-0a3b3843f0e1");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "3178903c-bf36-40f7-b870-724e238684ff");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "476127cb-62db-4af7-ac8e-d4a722f8e142");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "78b84ad9-6757-405a-9729-5d2af8615e07");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "907b7af0-b132-4951-a2dc-6ab82d4cd40d");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "be432c54-5860-4f56-832d-ceb3435d8b7e");
        }
    }
}
