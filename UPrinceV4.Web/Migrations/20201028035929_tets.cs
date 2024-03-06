using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class tets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PMolShortcutpaneData",
                keyColumn: "Id",
                keyValue: "4",
                column: "Value",
                value: -1);

            migrationBuilder.UpdateData(
                table: "PMolShortcutpaneData",
                keyColumn: "Id",
                keyValue: "9",
                column: "Value",
                value: -1);

            migrationBuilder.UpdateData(
              table: "PMolShortcutpaneData",
              keyColumn: "Id",
              keyValue: "5",
              column: "Value",
              value: -14);

            migrationBuilder.UpdateData(
            table: "PMolShortcutpaneData",
            keyColumn: "Id",
            keyValue: "10",
            column: "Value",
            value: -14);

            migrationBuilder.UpdateData(
           table: "PMolShortcutpaneData",
           keyColumn: "Id",
           keyValue: "13",
           column: "Value",
           value: 1);

           migrationBuilder.UpdateData(
           table: "PMolShortcutpaneData",
           keyColumn: "Id",
           keyValue: "14",
           column: "Value",
           value: 1);

          migrationBuilder.UpdateData(
          table: "PMolShortcutpaneData",
          keyColumn: "Id",
          keyValue: "15",
          column: "Value",
          value: -7);

         migrationBuilder.UpdateData(
         table: "PMolShortcutpaneData",
         keyColumn: "Id",
         keyValue: "16",
         column: "Value",
         value: -7);

            migrationBuilder.UpdateData(
       table: "PMolShortcutpaneData",
       keyColumn: "Id",
       keyValue: "17",
       column: "Value",
       value: -30);

            migrationBuilder.UpdateData(
       table: "PMolShortcutpaneData",
       keyColumn: "Id",
       keyValue: "18",
       column: "Value",
       value: -30);

            migrationBuilder.UpdateData(
    table: "PMolShortcutpaneData",
    keyColumn: "Id",
    keyValue: "19",
    column: "Value",
    value: -60);

            migrationBuilder.UpdateData(
       table: "PMolShortcutpaneData",
       keyColumn: "Id",
       keyValue: "20",
       column: "Value",
       value: -60);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
