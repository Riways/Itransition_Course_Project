using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class CommentsAndGrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<int>(
                name: "AuthorGrade",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Reviews",
                type: "text",
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.AddColumn<string>(
                name: "ReviewBody",
                table: "Reviews",
                type: "text",
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.AddColumn<string>(
                name: "SubjectOfReview",
                table: "Reviews",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<string>(type: "text", nullable: false),
                    ReviewModelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Comments", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Comments_Reviews_ReviewModelId",
                        column: x => x.ReviewModelId,
                        principalTable: "Reviews",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    ReviewId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Grades", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Grades_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Grades_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews",
                column: "AuthorId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Comments_ReviewModelId",
                table: "Comments",
                column: "ReviewModelId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Grades_AuthorId",
                table: "Grades",
                column: "AuthorId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Grades_ReviewId",
                table: "Grades",
                column: "ReviewId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_AuthorId",
                table: "Reviews",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_AuthorId",
                table: "Reviews");

            _ = migrationBuilder.DropTable(
                name: "Comments");

            _ = migrationBuilder.DropTable(
                name: "Grades");

            _ = migrationBuilder.DropIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews");

            _ = migrationBuilder.DropColumn(
                name: "AuthorGrade",
                table: "Reviews");

            _ = migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Reviews");

            _ = migrationBuilder.DropColumn(
                name: "ReviewBody",
                table: "Reviews");

            _ = migrationBuilder.DropColumn(
                name: "SubjectOfReview",
                table: "Reviews");
        }
    }
}
