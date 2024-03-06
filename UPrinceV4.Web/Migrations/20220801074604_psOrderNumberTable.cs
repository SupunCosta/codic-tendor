using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class psOrderNumberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractorPsOrderNumber",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorPsOrderNumber", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ContractorPsOrderNumber",
                columns: new[] { "Id", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "wer9e479-org1-Item-team1-e40dbe6a5w13", "en", "13", "2210e768-msms-kknk-jhhk-ee367a82ad13" },
                    { "wer9e479-org1-Item-team1-e40dbe6a5w17", "en", "17", "2210e768-msms-kknk-jhhk-ee367a82ad17" },
                    { "wer9e479-org1-Item-team1-e40dbe6a5we5", "en", "5", "2210e768-msms-kknk-jhhk-ee367a82ad25" },
                    { "wer9e479-org1-Item-team1-e40dbe6a5we9", "en", "9", "2210e768-msms-kknk-jhhk-ee367a82ad29" },
                    { "wer9e479-org1-Item-team1-e40dbe6a5wer", "en", "1", "2210e768-msms-kknk-jhhk-ee367a82ad21" },
                    { "wer9e479-org1-Item-team1-nl0dbe6a5w11", "en", "11", "2210e768-msms-kknk-jhhk-ee367a82ad11" },
                    { "wer9e479-org1-Item-team1-nl0dbe6a5w15", "en", "15", "2210e768-msms-kknk-jhhk-ee367a82ad15" },
                    { "wer9e479-org1-Item-team1-nl0dbe6a5w19", "en", "19", "2210e768-msms-kknk-jhhk-ee367a82ad19" },
                    { "wer9e479-org1-Item-team1-nl0dbe6a5we7", "en", "7", "2210e768-msms-kknk-jhhk-ee367a82ad27" },
                    { "wer9e479-org1-Item-team1-nl0dbe6a5wer", "en", "3", "2210e768-msms-kknk-jhhk-ee367a82ad23" },
                    { "wer9e479-org2-Item-team2-e40dbe6a5w10", "en", "10", "2210e768-msms-kknk-jhhk-ee367a82ad26" },
                    { "wer9e479-org2-Item-team2-e40dbe6a5w14", "en", "14", "2210e768-msms-kknk-jhhk-ee367a82ad14" },
                    { "wer9e479-org2-Item-team2-e40dbe6a5w18", "en", "18", "2210e768-msms-kknk-jhhk-ee367a82ad18" },
                    { "wer9e479-org2-Item-team2-e40dbe6a5we6", "en", "6", "2210e768-msms-kknk-jhhk-ee367a82ad26" },
                    { "wer9e479-org2-Item-team2-e40dbe6a5wer", "en", "2", "2210e768-msms-kknk-jhhk-ee367a82ad22" },
                    { "wer9e479-org2-Item-team2-nl0dbe6a5w12", "en", "12", "2210e768-msms-kknk-jhhk-ee367a82ad12" },
                    { "wer9e479-org2-Item-team2-nl0dbe6a5w16", "en", "16", "2210e768-msms-kknk-jhhk-ee367a82ad16" },
                    { "wer9e479-org2-Item-team2-nl0dbe6a5w20", "en", "20", "2210e768-msms-kknk-jhhk-ee367a82ad20" },
                    { "wer9e479-org2-Item-team2-nl0dbe6a5we8", "en", "8", "2210e768-msms-kknk-jhhk-ee367a82ad28" },
                    { "wer9e479-org2-Item-team2-nl0dbe6a5wer", "en", "4", "2210e768-msms-kknk-jhhk-ee367a82ad24" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractorPsOrderNumber");
        }
    }
}
