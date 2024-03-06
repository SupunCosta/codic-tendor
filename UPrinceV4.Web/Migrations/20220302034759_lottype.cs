using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lottype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentBudgetSpent",
                table: "ProjectFinance",
                newName: "CustomerBudgetSpent");

            migrationBuilder.AddColumn<string>(
                name: "ProjectManagerId",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LotProductItemType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotProductItemType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LotProductItemType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "wer9e479-msms-Item-Lot1-e40dbe6a5wer", 4, "en", "Lot", "2210e768-msms-Item-Lot1-ee367a82ad22" });

            migrationBuilder.InsertData(
                table: "LotProductItemType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[] { "wer9e479-msms-Item-Lot2-e40dbe6a5wer", 4, "en", "Contractor", "2210e768-msms-Item-Lot2-ee367a82ad22" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LotProductItemType");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "ProjectDefinition");

            migrationBuilder.RenameColumn(
                name: "CustomerBudgetSpent",
                table: "ProjectFinance",
                newName: "CurrentBudgetSpent");
        }
    }
}
