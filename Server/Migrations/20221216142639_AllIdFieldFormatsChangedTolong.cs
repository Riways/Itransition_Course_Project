using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace totten_romatoes.Server.Migrations
{
    public partial class AllIdFieldFormatsChangedTolong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "TagModel",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            _ = migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            _ = migrationBuilder.AlterColumn<long>(
                name: "TagsId",
                table: "ReviewModelTagModel",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            _ = migrationBuilder.AlterColumn<long>(
                name: "ReviewsId",
                table: "ReviewModelTagModel",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            _ = migrationBuilder.AlterColumn<long>(
                name: "ReviewId",
                table: "Grades",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            _ = migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Grades",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            _ = migrationBuilder.AlterColumn<long>(
                name: "ReviewModelId",
                table: "Comments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Comments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TagModel",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            _ = migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Reviews",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            _ = migrationBuilder.AlterColumn<int>(
                name: "TagsId",
                table: "ReviewModelTagModel",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            _ = migrationBuilder.AlterColumn<int>(
                name: "ReviewsId",
                table: "ReviewModelTagModel",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            _ = migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "Grades",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            _ = migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Grades",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            _ = migrationBuilder.AlterColumn<int>(
                name: "ReviewModelId",
                table: "Comments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            _ = migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Comments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
