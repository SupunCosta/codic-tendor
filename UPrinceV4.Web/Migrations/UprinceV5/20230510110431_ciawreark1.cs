#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class ciawreark1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CiawFeatchStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawFeatchStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CiawRemark",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CiawId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawRemark", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CiawFeatchStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { "1", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CiawFeatchStatus");

            migrationBuilder.DropTable(
                name: "CiawRemark");
        }
    }
}
