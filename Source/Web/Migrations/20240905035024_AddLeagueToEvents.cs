using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueToEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "League",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "League",
                table: "Events");
        }
    }
}
