using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class TSVectorAndDateOfCreationFieldAddedToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Reviews_ReviewModelId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ReviewModelId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReviewModelId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Comments");

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Reviews",
                type: "tsvector",
                nullable: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector")
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "ReviewBody", "Title" })
                .OldAnnotation("Npgsql:TsVectorConfig", "english")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "ReviewBody", "Title" });

            migrationBuilder.AddColumn<string>(
                name: "CommentBody",
                table: "Comments",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ReviewId",
                table: "Comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Comments",
                type: "tsvector",
                nullable: true)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "CommentBody" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReviewId",
                table: "Comments",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SearchVector",
                table: "Comments",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Reviews_ReviewId",
                table: "Comments",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Reviews_ReviewId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ReviewId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_SearchVector",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentBody",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Comments");

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Reviews",
                type: "tsvector",
                nullable: false,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "ReviewBody", "Title" })
                .OldAnnotation("Npgsql:TsVectorConfig", "english")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "ReviewBody", "Title" });

            migrationBuilder.AddColumn<long>(
                name: "ReviewModelId",
                table: "Comments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Comments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ReviewModelId",
                table: "Comments",
                column: "ReviewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Reviews_ReviewModelId",
                table: "Comments",
                column: "ReviewModelId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
