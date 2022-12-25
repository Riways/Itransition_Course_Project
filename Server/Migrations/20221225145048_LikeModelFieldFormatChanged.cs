using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class LikeModelFieldFormatChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Reviews_ReviewModelId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ReviewModelId",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "ReviewModelId",
                table: "Likes");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ReviewId",
                table: "Likes",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Reviews_ReviewId",
                table: "Likes",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Reviews_ReviewId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_ReviewId",
                table: "Likes");

            migrationBuilder.AddColumn<long>(
                name: "Review",
                table: "Likes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ReviewModelId",
                table: "Likes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ReviewModelId",
                table: "Likes",
                column: "ReviewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Reviews_ReviewModelId",
                table: "Likes",
                column: "ReviewModelId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
