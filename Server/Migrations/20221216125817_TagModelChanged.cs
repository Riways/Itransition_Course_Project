using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class TagModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagModel_Reviews_ReviewModelId",
                table: "TagModel");

            migrationBuilder.DropIndex(
                name: "IX_TagModel_ReviewModelId",
                table: "TagModel");

            migrationBuilder.DropColumn(
                name: "ReviewModelId",
                table: "TagModel");

            migrationBuilder.CreateTable(
                name: "ReviewModelTagModel",
                columns: table => new
                {
                    ReviewsId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewModelTagModel", x => new { x.ReviewsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ReviewModelTagModel_Reviews_ReviewsId",
                        column: x => x.ReviewsId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewModelTagModel_TagModel_TagsId",
                        column: x => x.TagsId,
                        principalTable: "TagModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewModelTagModel_TagsId",
                table: "ReviewModelTagModel",
                column: "TagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewModelTagModel");

            migrationBuilder.AddColumn<int>(
                name: "ReviewModelId",
                table: "TagModel",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagModel_ReviewModelId",
                table: "TagModel",
                column: "ReviewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagModel_Reviews_ReviewModelId",
                table: "TagModel",
                column: "ReviewModelId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
