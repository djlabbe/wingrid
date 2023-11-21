using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Season",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeasonSlug",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeasonType",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Week",
                table: "Events",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Season",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SeasonSlug",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SeasonType",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Week",
                table: "Events");
        }
    }
}
