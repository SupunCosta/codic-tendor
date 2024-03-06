using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UPrinceV4
{
    /// <inheritdoc />
    public partial class pbsLotId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PbsLotId",
                table: "PbsProduct",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PbsLotId",
                table: "PbsProduct");
        }
    }
}
