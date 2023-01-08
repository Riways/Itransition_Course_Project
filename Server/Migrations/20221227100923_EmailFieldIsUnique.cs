using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class EmailFieldIsUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            _ = migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Reviews");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subjects",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            _ = migrationBuilder.AddUniqueConstraint(
                name: "AK_Subjects_Name",
                table: "Subjects",
                column: "Name");

            _ = migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropUniqueConstraint(
                name: "AK_Subjects_Name",
                table: "Subjects");

            _ = migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subjects",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            _ = migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "Reviews",
                type: "bigint",
                nullable: true);

            _ = migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");
        }
    }
}
