using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class ImageModelCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<string>(
                name: "ReviewBody",
                table: "Reviews",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            _ = migrationBuilder.AddColumn<long>(
                name: "ImageId",
                table: "Reviews",
                type: "bigint",
                nullable: true);

            _ = migrationBuilder.AddColumn<long>(
                name: "ReviewImageId",
                table: "Reviews",
                type: "bigint",
                nullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            _ = migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageType = table.Column<string>(type: "text", nullable: false),
                    ImageName = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Images", x => x.Id);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewImageId",
                table: "Reviews",
                column: "ReviewImageId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Images_ReviewImageId",
                table: "Reviews",
                column: "ReviewImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Images_ReviewImageId",
                table: "Reviews");

            _ = migrationBuilder.DropTable(
                name: "Images");

            _ = migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewImageId",
                table: "Reviews");

            _ = migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Reviews");

            _ = migrationBuilder.DropColumn(
                name: "ReviewImageId",
                table: "Reviews");

            _ = migrationBuilder.AlterColumn<string>(
                name: "ReviewBody",
                table: "Reviews",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(5000)",
                oldMaxLength: 5000);

            _ = migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            _ = migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);
        }
    }
}
