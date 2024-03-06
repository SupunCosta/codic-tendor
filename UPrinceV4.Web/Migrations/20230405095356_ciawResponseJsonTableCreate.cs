using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations
{
    /// <inheritdoc />
    public partial class ciawResponseJsonTableCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CiawCancelStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawCancelStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CiawResponseJson",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CiawReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuccessJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawResponseJson", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CiawCancelStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "77143c60-ff00-cancl-ciaws-213d2d1c5f5a", "3", "nl", "unemployment(temporary)(nl)", "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce" },
                    { "77143c60-ff45-4ca2-ciaws-213d2d1c5f5a", "2", "en", "Illness", "7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce" },
                    { "77143c60-ff45-4ca2-ciaws-213d2d1c5fnl", "2", "nl", "Illness(nl)", "7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce" },
                    { "77143c60-ff45-cancl-ciaws-213d2d1c5f5a", "3", "en", "unemployment(temporary)", "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce" },
                    { "77143c60-ff45-cancl-ciaws-213d2d1c5f6b", "4", "nl", "Change in schedule(nl)", "7bcb4e8d-cancl-487d-ciaws-6b99c99fAbbe" },
                    { "77143c60-ff45-cancl-ciaws-213d2d1c5g7g", "4", "en", "Change in schedule", "7bcb4e8d-cancl-487d-ciaws-6b99c99fAbbe" },
                    { "bdd9e479-75b3-40c6-ciaws-e40dbe6a51ac", "1", "en", "Holidays", "4010e768-3e06-4702-ciaws-ee367a82addb" },
                    { "bdd9e479-75b3-40c6-ciaws-e40dbe6a51nl", "1", "nl", "Holidays(nl)", "4010e768-3e06-4702-ciaws-ee367a82addb" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CiawCancelStatus");

            migrationBuilder.DropTable(
                name: "CiawResponseJson");
        }
    }
}
