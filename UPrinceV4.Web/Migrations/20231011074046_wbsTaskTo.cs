using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class wbsTaskTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "WbsTask");

            migrationBuilder.DropColumn(
                name: "InstructionId",
                table: "WbsTask");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "WbsTask",
                newName: "WbsTaxonomyId");

            migrationBuilder.CreateTable(
                name: "WbsTaskTo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTaskTo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WbsTaskTo");

            migrationBuilder.RenameColumn(
                name: "WbsTaxonomyId",
                table: "WbsTask",
                newName: "To");

            migrationBuilder.AddColumn<string>(
                name: "EmailId",
                table: "WbsTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionId",
                table: "WbsTask",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
