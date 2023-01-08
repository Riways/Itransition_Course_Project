using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class TagModelAndDateOfCreationInReviewModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreationInUTC",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.CreateTable(
                name: "TagModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ReviewModelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_TagModel", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_TagModel_Reviews_ReviewModelId",
                        column: x => x.ReviewModelId,
                        principalTable: "Reviews",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_TagModel_ReviewModelId",
                table: "TagModel",
                column: "ReviewModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "TagModel");

            _ = migrationBuilder.DropColumn(
                name: "DateOfCreationInUTC",
                table: "Reviews");
        }
    }
}
