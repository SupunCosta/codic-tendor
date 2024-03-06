using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class wbsTaskStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "WbsTask",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryStatusId",
                table: "WbsTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "WbsTask",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StatusId",
                table: "WbsTask",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WbsTaskDeliveryStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTaskDeliveryStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WbsTaskEmail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTaskEmail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WbsTaskStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTaskStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WbsTaskDeliveryStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "bcfe479-msms-4ZIP-Lot5-e40dbe6a5wer", 2, "nl", "More than 7 days(nl)", "12a22319-8ca7-temp-b588-6fab99474130" },
                    { "d263aa4e-12a8-issu-bc99-d561a603748e", 0, "en", "Overdue", "i1ce52c0-058b-issu-afbd-1d2d24105ebc" },
                    { "lsj3aa4e-12a8-issu-bc99-d561a603748e", 0, "nl", "Overdue(nl)", "i1ce52c0-058b-issu-afbd-1d2d24105ebc" },
                    { "poie479-msms-40c6-Lot5-e40dbe6a5wer", 1, "nl", "Within 7 Days(nl)", "2210e768-msms-po02-Lot5-ee367a82ad22" },
                    { "wer9e479-msms-40c6-Lot5-e40dbe6a5wer", 1, "en", "Within 7 Days", "2210e768-msms-po02-Lot5-ee367a82ad22" },
                    { "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer", 2, "en", "More than 7 days", "12a22319-8ca7-temp-b588-6fab99474130" }
                });

            migrationBuilder.InsertData(
                table: "WbsTaskStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "273D6023-0643-4F16-8605-652AF0B658A2", 3, "en", "three quater", "5684969c-f3e8-49ac-9746-51e7e23f2782" },
                    { "d263aa4e-12a8-issu-bc99-d561a603748e", 0, "en", "full", "0e1b34bc-f2c3-4778-8250-9666ee96ae59" },
                    { "ehwraa4e-12a8-issu-bc99-d561a603748e", 0, "nl", "full", "0e1b34bc-f2c3-4778-8250-9666ee96ae59" },
                    { "lksdm479-msms-4ZIP-Lot5-e40dbe6a5wer", 2, "nl", "half", "40843898-54EE-473D-A661-194F1DA0CE48" },
                    { "wecv6023-0643-4F16-8605-652AF0B658A2", 3, "nl", "three quater", "5684969c-f3e8-49ac-9746-51e7e23f2782" },
                    { "wer9e479-msms-40c6-Lot5-e40dbe6a5wer", 1, "en", "quater", "3960193f-99e0-43c6-a6cc-4919e5d345c5" },
                    { "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer", 2, "en", "half", "40843898-54EE-473D-A661-194F1DA0CE48" },
                    { "wfede479-msms-40c6-Lot5-e40dbe6a5wer", 1, "nl", "quater", "3960193f-99e0-43c6-a6cc-4919e5d345c5" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WbsTaskDeliveryStatus");

            migrationBuilder.DropTable(
                name: "WbsTaskEmail");

            migrationBuilder.DropTable(
                name: "WbsTaskStatus");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "WbsTask");

            migrationBuilder.DropColumn(
                name: "DeliveryStatusId",
                table: "WbsTask");

            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "WbsTask");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "WbsTask");
        }
    }
}
