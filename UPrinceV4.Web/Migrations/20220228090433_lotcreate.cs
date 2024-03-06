using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lotcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "BMLotContractorsList");

            migrationBuilder.DropColumn(
                name: "InvitationMail",
                table: "BMLotContractorsList");

            migrationBuilder.RenameColumn(
                name: "Staus",
                table: "BMLotHeader",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "ProductItemType",
                table: "BMLotHeader",
                newName: "ProductItemTypeId");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "BMLotContractorsList",
                newName: "ContractorTeamId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BMLotContractorsList",
                newName: "ContractorLot");

            migrationBuilder.CreateTable(
                name: "BMLotContractorTeamList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvitationMail = table.Column<bool>(type: "bit", nullable: true),
                    CabPersonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CabPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotContractorTeamList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotTenderAward",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWinner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotTenderAward", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BMLotContractorTeamList");

            migrationBuilder.DropTable(
                name: "BMLotTenderAward");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "BMLotHeader",
                newName: "Staus");

            migrationBuilder.RenameColumn(
                name: "ProductItemTypeId",
                table: "BMLotHeader",
                newName: "ProductItemType");

            migrationBuilder.RenameColumn(
                name: "ContractorTeamId",
                table: "BMLotContractorsList",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "ContractorLot",
                table: "BMLotContractorsList",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "BMLotContractorsList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InvitationMail",
                table: "BMLotContractorsList",
                type: "bit",
                nullable: true);
        }
    }
}
