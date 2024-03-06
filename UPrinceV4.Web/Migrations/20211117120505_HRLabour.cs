using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class HRLabour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HRRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRRoles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HRRoles",
                columns: new[] { "Id", "Label", "LanguageCode", "Name", "RoleId" },
                values: new object[,]
                {
                    { "uu5ab9fe-po57-4088-82a9-d27008688bvv", "Manager", "en", "Manager", "115ab9fe-po57-4088-82a9-d27008688bvv" },
                    { "lld9e479-pob3-40c6-ad61-e40dbe6a51uu", "Manager", "nl", "Manager(nl)", "115ab9fe-po57-4088-82a9-d27008688bvv" },
                    { "jj5ab9fe-po57-4088-82a9-d27008688tgg", "Worker", "en", "Worker", "335ab9fe-po57-4088-82a9-d27008688tgg" },
                    { "ffd9e479-pob3-40c6-ad61-e40dbe6a54kk", "Worker", "nl", "Worker(nl)", "335ab9fe-po57-4088-82a9-d27008688tgg" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRRoles");
        }
    }
}
