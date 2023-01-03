using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class LikeAddedForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Likes_FromUserId",
                table: "Likes");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Likes_FromUserId_ReviewId",
                table: "Likes",
                columns: new[] { "FromUserId", "ReviewId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Likes_FromUserId_ReviewId",
                table: "Likes");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_FromUserId",
                table: "Likes",
                column: "FromUserId");
        }
    }
}
