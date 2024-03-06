﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedDateToInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Invoice",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Invoice");
        }
    }
}
