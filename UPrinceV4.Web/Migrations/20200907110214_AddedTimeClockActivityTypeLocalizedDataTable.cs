using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedTimeClockActivityTypeLocalizedDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeClockActivityTypeLocalizedData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    LanguageCode = table.Column<string>(nullable: true),
                    TimeClockActivityTypeId = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeClockActivityTypeLocalizedData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeClockActivityTypeLocalizedData");
        }
    }
}
