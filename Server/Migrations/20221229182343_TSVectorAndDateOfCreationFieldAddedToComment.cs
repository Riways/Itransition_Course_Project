using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class TSVectorAndDateOfCreationFieldAddedToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Comments_Reviews_ReviewModelId",
                table: "Comments");

            _ = migrationBuilder.DropIndex(
                name: "IX_Comments_ReviewModelId",
                table: "Comments");

            _ = migrationBuilder.DropColumn(
                name: "ReviewModelId",
                table: "Comments");

            _ = migrationBuilder.DropColumn(
                name: "Text",
                table: "Comments");

            _ = migrationBuilder.AlterColumn<NpgsqlTsVector>(
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

            _ = migrationBuilder.AddColumn<string>(
                name: "CommentBody",
                table: "Comments",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.AddColumn<long>(
                name: "ReviewId",
                table: "Comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            _ = migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Comments",
                type: "tsvector",
                nullable: true)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "CommentBody" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comments_ReviewId",
                table: "Comments",
                column: "ReviewId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comments_SearchVector",
                table: "Comments",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Comments_Reviews_ReviewId",
                table: "Comments",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Comments_Reviews_ReviewId",
                table: "Comments");

            _ = migrationBuilder.DropIndex(
                name: "IX_Comments_ReviewId",
                table: "Comments");

            _ = migrationBuilder.DropIndex(
                name: "IX_Comments_SearchVector",
                table: "Comments");

            _ = migrationBuilder.DropColumn(
                name: "CommentBody",
                table: "Comments");

            _ = migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Comments");

            _ = migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Comments");

            _ = migrationBuilder.AlterColumn<NpgsqlTsVector>(
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

            _ = migrationBuilder.AddColumn<long>(
                name: "ReviewModelId",
                table: "Comments",
                type: "bigint",
                nullable: true);

            _ = migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Comments",
                type: "text",
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comments_ReviewModelId",
                table: "Comments",
                column: "ReviewModelId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Comments_Reviews_ReviewModelId",
                table: "Comments",
                column: "ReviewModelId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
