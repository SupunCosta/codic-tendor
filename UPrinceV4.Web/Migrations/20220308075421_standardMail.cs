using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class standardMail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StandardMailHeader",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MailHeader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestToWrittenInTender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasuringStateRecieved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reminder1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reminder1TimeFrameTender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reminder2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reminder2TimeFrameTender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reminder3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reminder3TimeFrameTender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenderWon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenderLost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutStandingComments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardMailHeader", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StandardMailHeader");
        }
    }
}
