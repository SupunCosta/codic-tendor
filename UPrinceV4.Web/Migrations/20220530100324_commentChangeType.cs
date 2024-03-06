using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class commentChangeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentChangeType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentChangeType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CommentChangeType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "d21f11f8-cf12-46ca-85c8-804faf7e70da", 3, "en", "Issue", "c2ab9d4e-c4ca-4b99-8bf7-38597f6160f1" },
                    { "d4f6116f-5026-424b-94c3-e404d39db195", 3, "nl", "Kwestie", "c2ab9d4e-c4ca-4b99-8bf7-38597f6160f1" },
                    { "hkdsg-jsjj-fmms-amdm-b7a93ebrghthh", 1, "en", "Change Request", "bbbbk4e8d-8e8c-487d-8170-6b91c89fcbbb" },
                    { "kkk93-jsjj-fmms-amdm-b7a93ebd1wem", 1, "nl", "veranderingsaanvraag", "bbbbk4e8d-8e8c-487d-8170-6b91c89fcbbb" },
                    { "ppp93-jsjj-fmms-amdm-b7a93ebd1wem", 2, "en", "Comment", "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv" },
                    { "zzz93-jsjj-fmms-amdm-b7a93ebd1wem", 2, "nl", "commentaar", "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentChangeType");
        }
    }
}
