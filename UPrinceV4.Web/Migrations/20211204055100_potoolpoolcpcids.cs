using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class potoolpoolcpcids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CPCId",
                table: "POToolPool",
                newName: "RequestedCPCId");

            migrationBuilder.AddColumn<string>(
                name: "AssignedCPCId",
                table: "POToolPool",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedCPCId",
                table: "POToolPool");

            migrationBuilder.RenameColumn(
                name: "RequestedCPCId",
                table: "POToolPool",
                newName: "CPCId");
        }
    }
}
