using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class commentlogdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "CommentCard",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "CommentCard");
        }
    }
}
