﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class TypeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StockType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 1, "en", "Materials", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3", 2, "en", "Tools", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae1", 3, "en", "Consumables", "94282458-0b40-40a3-b0f9-c2e40344c8f1" }
                });

            migrationBuilder.InsertData(
                table: "WFActivityStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-b337-ee367a82addb" },
                    { "813a0c70-b58f-433d-8945-9cb166ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-cacdfecdb84c" },
                    { "77143c60-ff45-4ca2-8723-213d2d1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "4e01a893-0267-48af-b65a-b7a93ebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" },
                    { "2732cd5a-0941-4c56-9c13-f5fdca2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "098cf409-7cb8-4076-8ddf-657dd897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-ad25-618d80d49477" },
                    { "5015743d-a2e6-4531-808d-d4e1400e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-cacdfecdb84c" }
                });

            migrationBuilder.InsertData(
                table: "WFType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 1, "en", "To be Assigned", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3", 2, "en", "Finished", "4010e768-3e06-4702-b337-ee367a82addb" }
                });

            migrationBuilder.InsertData(
                table: "WHType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "a35ab9fe-df57-4088-82a9-d27008688bae", 1, "en", "Fixed", "94282458-0b40-40a3-b0f9-c2e40344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3", 2, "en", "Mobile", "4010e768-3e06-4702-b337-ee367a82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "StockType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae1");

            migrationBuilder.DeleteData(
                table: "StockType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "098cf409-7cb8-4076-8ddf-657dd897f5bb");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "12e2d6c5-1ada-4e74-88ba-ce7fbf10e27c");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "2732cd5a-0941-4c56-9c13-f5fdca2ab276");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "4e01a893-0267-48af-b65a-b7a93ebd1ccf");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "5015743d-a2e6-4531-808d-d4e1400e1eed");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-4ca2-8723-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "813a0c70-b58f-433d-8945-9cb166ae42af");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "8d109134-ee8d-4c7b-84c5-dd1bf1e2391a");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "WFActivityStatus",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac");

            migrationBuilder.DeleteData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "WFType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3");

            migrationBuilder.DeleteData(
                table: "WHType",
                keyColumn: "Id",
                keyValue: "a35ab9fe-df57-4088-82a9-d27008688bae");

            migrationBuilder.DeleteData(
                table: "WHType",
                keyColumn: "Id",
                keyValue: "bdd9e479-75b3-40c6-ad61-e40dbe6a51ac3");
        }
    }
}
