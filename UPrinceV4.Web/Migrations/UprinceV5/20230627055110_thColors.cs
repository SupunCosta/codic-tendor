#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class thColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThColors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThColors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ThColors",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { "1", "#c56834" },
                    { "10", "#f69b68" },
                    { "11", "#b3da90" },
                    { "12", "#b65ab3" },
                    { "13", "#08272B" },
                    { "14", "#19D65B" },
                    { "15", "#1C6675" },
                    { "16", "#AE9675" },
                    { "17", "#8C2581" },
                    { "18", "#7EF6CE" },
                    { "19", "#1C85CF" },
                    { "2", "#c4d8e5" },
                    { "20", "#0F264F" },
                    { "3", "#97c8ea" },
                    { "4", "#3b9b36" },
                    { "5", "#a5982c" },
                    { "6", "#97ac0f" },
                    { "7", "#b13748" },
                    { "8", "#ea716d" },
                    { "9", "#166fdb" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThColors");
        }
    }
}
