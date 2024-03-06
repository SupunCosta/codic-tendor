using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedDuctchWordstoPmolStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "Name",
                value: "in voorbereiding");

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "Name",
                value: "goedgekeurd");

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed",
                column: "Name",
                value: "ter goedkeuring");

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "Name",
                value: "in uitvoering");

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
                column: "Name",
                value: "afgewerkt en doorgegeven");

            
            migrationBuilder.Sql(" update PbsProductStatusLocalizedData set Label = 'in voorbereiding' where Id = '098cf409-7cb8-4076-8ddf-657dd897f5bb'");
            migrationBuilder.Sql("update PbsProductStatusLocalizedData set Label = 'in uitvoering' where Id = 'a35ab9fe-df57-4088-82a9-d27008688bae'");
            migrationBuilder.Sql("update PbsProductStatusLocalizedData set Label = 'ter goedkeuring' where Id = '5015743d-a2e6-4531-808d-d4e1400e1eed'");
            migrationBuilder.Sql("update PbsProductStatusLocalizedData set Label = 'goedgekeurd' where Id = '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'");
            migrationBuilder.Sql("update PbsProductStatusLocalizedData set Label = 'afgewerkt en doorgegeven' where Id = '4010e768-3e06-4702-b337-ee367a82addb'");

            migrationBuilder.Sql("update PbsProductItemTypeLocalizedData set Label = 'Goddeeris' where Id = 'a33d78ab-f64d-4f27-9ef8-7023e3fa7add'");
            migrationBuilder.Sql("update PbsProductItemTypeLocalizedData set Label = 'Goddeeris' where Id = '71987b81-619d-421d-a8d4-d7944aa277b3'");
            migrationBuilder.Sql("update PbsProductItemTypeLocalizedData set Label = 'Subcontractor' where Id = '1ec0b1e6-fe13-454f-a5ac-25ade370c985'");
            migrationBuilder.Sql("update PbsProductItemTypeLocalizedData set Label = 'Onderaannemer' where Id = '60a8b16a-c2cc-4ed3-96eb-e16151fd5915'");

            migrationBuilder.Sql("update ProjectToleranceState set Name = 'Haalbaar' where Id = '677df1a3-f3e1-4192-9d1b-1be280312e86'");
            migrationBuilder.Sql("update ProjectToleranceState set Name = 'Haalbaar' where Id = 'c76bd93f-c1ef-4412-9f7e-4cbd24e84b83'");
            migrationBuilder.Sql("update ProjectToleranceState set Name = 'Nog net haalbaar' where Id = '4cbbc16d-3e81-4939-afe5-e418fbb97572'");
            migrationBuilder.Sql("update ProjectToleranceState set Name = 'Nog net haalbaar' where Id = 'ab780908-c71e-45ff-9639-b13d299b6222'");
            migrationBuilder.Sql("update ProjectToleranceState set Name = 'Niet meer Haalbaar' where Id = '72263030-866c-4342-9ff2-f295a2c20c47'");
            migrationBuilder.Sql("update ProjectToleranceState set Name = 'Niet meer Haalbaar' where Id = 'e2fefdc7-9cab-4088-9085-f699a0c34f3e'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb",
                column: "Name",
                value: "Pending Development(nl)");

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf",
                column: "Name",
                value: "Approved(nl)");

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed",
                column: "Name",
                value: "In Review(nl)");

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae",
                column: "Name",
                value: "In Development(nl)");

            migrationBuilder.UpdateData(
                table: "PMolStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac",
                column: "Name",
                value: "Handed Over(nl)");
        }
    }
}
