using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class TagModelTableRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_ReviewModelTagModel_TagModel_TagsId",
                table: "ReviewModelTagModel");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_TagModel",
                table: "TagModel");

            _ = migrationBuilder.RenameTable(
                name: "TagModel",
                newName: "Tags");

            _ = migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Images",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ReviewModelTagModel_Tags_TagsId",
                table: "ReviewModelTagModel",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_ReviewModelTagModel_Tags_TagsId",
                table: "ReviewModelTagModel");

            _ = migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            _ = migrationBuilder.RenameTable(
                name: "Tags",
                newName: "TagModel");

            _ = migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            _ = migrationBuilder.AddPrimaryKey(
                name: "PK_TagModel",
                table: "TagModel",
                column: "Id");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_ReviewModelTagModel_TagModel_TagsId",
                table: "ReviewModelTagModel",
                column: "TagsId",
                principalTable: "TagModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
