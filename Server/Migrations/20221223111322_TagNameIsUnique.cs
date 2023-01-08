using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class TagNameIsUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AddUniqueConstraint(
                name: "AK_Tags_Name",
                table: "Tags",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropUniqueConstraint(
                name: "AK_Tags_Name",
                table: "Tags");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(40)",
                oldMaxLength: 40);
        }
    }
}
