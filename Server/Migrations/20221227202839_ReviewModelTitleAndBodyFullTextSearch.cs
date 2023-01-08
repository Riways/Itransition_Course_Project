using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class ReviewModelTitleAndBodyFullTextSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Reviews",
                type: "tsvector",
                nullable: false)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "ReviewBody", "Title" });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Reviews_SearchVector",
                table: "Reviews",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "IX_Reviews_SearchVector",
                table: "Reviews");

            _ = migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Reviews");
        }
    }
}
