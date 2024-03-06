using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PbsinstructionPicklistAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "PbsInstructionFamilyId",
                table: "PbsInstruction",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PbsInstructionFamily",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Family = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsInstructionFamily", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsInstructionLink",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    PbsInstructionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsInstructionLink", x => x.Id);
                });



        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbsInstructionFamily");

            migrationBuilder.DropTable(
                name: "PbsInstructionLink");

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1676);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1677);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1678);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1679);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1680);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1681);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1682);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1683);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1684);

            migrationBuilder.DeleteData(
                table: "LocalizedData",
                keyColumn: "Id",
                keyValue: 1685);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PbsSkill");

            migrationBuilder.DropColumn(
                name: "PbsInstructionFamilyId",
                table: "PbsInstruction");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PbsSkill",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
