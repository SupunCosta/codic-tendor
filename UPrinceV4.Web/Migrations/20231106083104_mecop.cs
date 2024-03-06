using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class mecop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mecop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AankoopOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lijn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandWeerstand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Materiaal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grondlaag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ral = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ncs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MuurDikte = table.Column<double>(type: "float", nullable: false),
                    BreedteDeurlijsten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BreedteDeurlijstenVerticaal = table.Column<double>(type: "float", nullable: false),
                    BreedteDeurlijstenHorizontaal = table.Column<double>(type: "float", nullable: false),
                    OmlijstingenNummeren = table.Column<bool>(type: "bit", nullable: false),
                    HoogteDeur = table.Column<double>(type: "float", nullable: false),
                    BreedteDeur = table.Column<double>(type: "float", nullable: false),
                    BreedteVleugel = table.Column<double>(type: "float", nullable: false),
                    DikteDeur = table.Column<double>(type: "float", nullable: false),
                    DraaiRichting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Paumelles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AantalPaumelles = table.Column<int>(type: "int", nullable: false),
                    TegenPlaat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlotUitsparing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KrukHoogte = table.Column<double>(type: "float", nullable: false),
                    KabelDoorvoer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoogteKabelDoorvoer = table.Column<double>(type: "float", nullable: false),
                    Extra1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extra2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extra3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extra4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeurNummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aantal = table.Column<int>(type: "int", nullable: false),
                    Eenheidsprijs = table.Column<double>(type: "float", nullable: false),
                    TotaalPrijs = table.Column<double>(type: "float", nullable: false),
                    Taal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Klant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Norm = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mecop", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mecop");
        }
    }
}
