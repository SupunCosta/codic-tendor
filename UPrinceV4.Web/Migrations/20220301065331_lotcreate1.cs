using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lotcreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContractorTeamId",
                table: "BMLotContractorsList",
                newName: "LotId");

            migrationBuilder.AddColumn<string>(
                name: "LotId",
                table: "BMLotTenderAward",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotContractorId",
                table: "BMLotContractorTeamList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "BMLotContractorTeamList",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LotId",
                table: "BMLotTenderAward");

            migrationBuilder.DropColumn(
                name: "LotContractorId",
                table: "BMLotContractorTeamList");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "BMLotContractorTeamList");

            migrationBuilder.RenameColumn(
                name: "LotId",
                table: "BMLotContractorsList",
                newName: "ContractorTeamId");
        }
    }
}
