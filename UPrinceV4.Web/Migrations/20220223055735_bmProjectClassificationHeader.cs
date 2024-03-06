using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class bmProjectClassificationHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentBudgetSpent",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerBudget",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DifferenceEstimatedCostAndTenderBudget",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DifferenceTenderAndCustomer",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectedTotalProjectCost",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraWork",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinAndExtraWork",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinWork",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenderBudget",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToBeInvoiced",
                table: "ProjectFinance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectClassification",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectClassificationBuisnessUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectClassificationSizeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectClassificationConstructionTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectClassificationSectorId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectClassification", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectClassification");

            migrationBuilder.DropColumn(
                name: "CurrentBudgetSpent",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "CustomerBudget",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "DifferenceEstimatedCostAndTenderBudget",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "DifferenceTenderAndCustomer",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "ExpectedTotalProjectCost",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "ExtraWork",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "MinAndExtraWork",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "MinWork",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "TenderBudget",
                table: "ProjectFinance");

            migrationBuilder.DropColumn(
                name: "ToBeInvoiced",
                table: "ProjectFinance");
        }
    }
}
