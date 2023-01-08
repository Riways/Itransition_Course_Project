using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class LikeModelFieldFormatChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Likes_Reviews_ReviewModelId",
                table: "Likes");

            _ = migrationBuilder.DropIndex(
                name: "IX_Likes_ReviewModelId",
                table: "Likes");

            _ = migrationBuilder.DropColumn(
                name: "Review",
                table: "Likes");

            _ = migrationBuilder.DropColumn(
                name: "ReviewModelId",
                table: "Likes");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Likes_ReviewId",
                table: "Likes",
                column: "ReviewId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Likes_Reviews_ReviewId",
                table: "Likes",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Likes_Reviews_ReviewId",
                table: "Likes");

            _ = migrationBuilder.DropIndex(
                name: "IX_Likes_ReviewId",
                table: "Likes");

            _ = migrationBuilder.AddColumn<long>(
                name: "Review",
                table: "Likes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            _ = migrationBuilder.AddColumn<long>(
                name: "ReviewModelId",
                table: "Likes",
                type: "bigint",
                nullable: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Likes_ReviewModelId",
                table: "Likes",
                column: "ReviewModelId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Likes_Reviews_ReviewModelId",
                table: "Likes",
                column: "ReviewModelId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
