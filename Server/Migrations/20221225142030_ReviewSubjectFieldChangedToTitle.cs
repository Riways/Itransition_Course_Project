using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class ReviewSubjectFieldChangedToTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.RenameColumn(
                name: "SubjectOfReview",
                table: "Reviews",
                newName: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.RenameColumn(
                name: "Title",
                table: "Reviews",
                newName: "SubjectOfReview");
        }
    }
}
