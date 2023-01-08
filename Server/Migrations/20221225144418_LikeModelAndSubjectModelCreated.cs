using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class LikeModelAndSubjectModelCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Grades_Reviews_ReviewId",
                table: "Grades");

            _ = migrationBuilder.DropIndex(
                name: "IX_Grades_ReviewId",
                table: "Grades");

            _ = migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Grades",
                newName: "SubjectId");

            _ = migrationBuilder.AddColumn<long>(
                name: "SubjectId",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            _ = migrationBuilder.AddUniqueConstraint(
                name: "AK_Grades_SubjectId_AuthorId",
                table: "Grades",
                columns: new[] { "SubjectId", "AuthorId" });

            _ = migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromUserId = table.Column<string>(type: "text", nullable: false),
                    ToUserId = table.Column<string>(type: "text", nullable: false),
                    ReviewId = table.Column<long>(type: "bigint", nullable: false),
                    Review = table.Column<long>(type: "bigint", nullable: false),
                    ReviewModelId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Likes", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Likes_Reviews_ReviewModelId",
                        column: x => x.ReviewModelId,
                        principalTable: "Reviews",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_Reviews_SubjectId",
                table: "Reviews",
                column: "SubjectId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Likes_FromUserId",
                table: "Likes",
                column: "FromUserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Likes_ReviewModelId",
                table: "Likes",
                column: "ReviewModelId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Likes_ToUserId",
                table: "Likes",
                column: "ToUserId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Grades_Subjects_SubjectId",
                table: "Grades",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Subjects_SubjectId",
                table: "Reviews",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Grades_Subjects_SubjectId",
                table: "Grades");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Subjects_SubjectId",
                table: "Reviews");

            _ = migrationBuilder.DropTable(
                name: "Likes");

            _ = migrationBuilder.DropTable(
                name: "Subjects");

            _ = migrationBuilder.DropIndex(
                name: "IX_Reviews_SubjectId",
                table: "Reviews");

            _ = migrationBuilder.DropUniqueConstraint(
                name: "AK_Grades_SubjectId_AuthorId",
                table: "Grades");

            _ = migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Reviews");

            _ = migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Grades",
                newName: "ReviewId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Grades_ReviewId",
                table: "Grades",
                column: "ReviewId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Grades_Reviews_ReviewId",
                table: "Grades",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
