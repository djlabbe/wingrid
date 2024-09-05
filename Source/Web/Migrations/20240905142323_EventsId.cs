using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.Migrations
{
    /// <inheritdoc />
    public partial class EventsId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Entries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Entries",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
