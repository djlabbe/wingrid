using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.EventAPI.Migrations
{
    /// <inheritdoc />
    public partial class TeamLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Teams",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Teams");
        }
    }
}
