using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class TagModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_TagModel_Reviews_ReviewModelId",
                table: "TagModel");

            _ = migrationBuilder.DropIndex(
                name: "IX_TagModel_ReviewModelId",
                table: "TagModel");

            _ = migrationBuilder.DropColumn(
                name: "ReviewModelId",
                table: "TagModel");

            _ = migrationBuilder.CreateTable(
                name: "ReviewModelTagModel",
                columns: table => new
                {
                    ReviewsId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_ReviewModelTagModel", x => new { x.ReviewsId, x.TagsId });
                    _ = table.ForeignKey(
                        name: "FK_ReviewModelTagModel_Reviews_ReviewsId",
                        column: x => x.ReviewsId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_ReviewModelTagModel_TagModel_TagsId",
                        column: x => x.TagsId,
                        principalTable: "TagModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_ReviewModelTagModel_TagsId",
                table: "ReviewModelTagModel",
                column: "TagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "ReviewModelTagModel");

            _ = migrationBuilder.AddColumn<int>(
                name: "ReviewModelId",
                table: "TagModel",
                type: "integer",
                nullable: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_TagModel_ReviewModelId",
                table: "TagModel",
                column: "ReviewModelId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_TagModel_Reviews_ReviewModelId",
                table: "TagModel",
                column: "ReviewModelId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
