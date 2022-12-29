using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class DateCommentFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreationInUTC",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfCreationInUTC",
                table: "Comments");
        }
    }
}
