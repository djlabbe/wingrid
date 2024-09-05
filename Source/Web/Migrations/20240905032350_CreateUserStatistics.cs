using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserStatistics",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    TotalCollegePicks = table.Column<int>(type: "integer", nullable: false),
                    TotalProPicks = table.Column<int>(type: "integer", nullable: false),
                    CorrectCollegePicks = table.Column<int>(type: "integer", nullable: false),
                    CorrectProPicks = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatistics", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserStatistics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStatistics");
        }
    }
}
