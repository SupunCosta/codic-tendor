using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class hrContractList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HRContractorList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HRId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRContractorList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRContractTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRContractTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HRContractTypes",
                columns: new[] { "Id", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "005b42ba-574d-4afd-a034-347858e53c9d", "en", "Tempory", "12a22319-8ca7-temp-b588-6fab99474130" },
                    { "1377a17d-3f18-46c1-bc7c-c11edcf65b5c", "nl", "Tempory(nl)", "12a22319-8ca7-temp-b588-6fab99474130" },
                    { "222e3dab-576d-4f53-b976-a9b5c97ee165", "en", "Permenant", "41ce52c0-058b-perm-afbd-1d2d24105ebc" },
                    { "3263aa4e-12a8-4c59-bc99-d561a603748e", "nl", "Permenant(nl)", "41ce52c0-058b-perm-afbd-1d2d24105ebc" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRContractorList");

            migrationBuilder.DropTable(
                name: "HRContractTypes");
        }
    }
}
