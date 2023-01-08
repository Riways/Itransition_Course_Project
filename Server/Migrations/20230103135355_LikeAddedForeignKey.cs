using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class LikeAddedForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "IX_Likes_FromUserId",
                table: "Likes");

            _ = migrationBuilder.AddUniqueConstraint(
                name: "AK_Likes_FromUserId_ReviewId",
                table: "Likes",
                columns: new[] { "FromUserId", "ReviewId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropUniqueConstraint(
                name: "AK_Likes_FromUserId_ReviewId",
                table: "Likes");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Likes_FromUserId",
                table: "Likes",
                column: "FromUserId");
        }
    }
}
