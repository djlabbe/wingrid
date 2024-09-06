using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.Migrations
{
    /// <inheritdoc />
    public partial class NewUserStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Entries",
                table: "UserStatistics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTieBreakerError",
                table: "UserStatistics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "UserStatistics",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entries",
                table: "UserStatistics");

            migrationBuilder.DropColumn(
                name: "TotalTieBreakerError",
                table: "UserStatistics");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "UserStatistics");
        }
    }
}
