using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class isWinnerForContractorTotalValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContractorId",
                table: "CommentCard",
                newName: "CompanyId");

            migrationBuilder.AddColumn<bool>(
                name: "IsWinner",
                table: "ContractorTotalValues",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWinner",
                table: "ContractorTotalValues");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "CommentCard",
                newName: "ContractorId");
        }
    }
}
