using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.Migrations
{
    /// <inheritdoc />
    public partial class FixtureEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Fixtures_FixtureId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_FixtureId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FixtureId",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "EventFixture",
                columns: table => new
                {
                    EventsId = table.Column<string>(type: "text", nullable: false),
                    FixturesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventFixture", x => new { x.EventsId, x.FixturesId });
                    table.ForeignKey(
                        name: "FK_EventFixture_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventFixture_Fixtures_FixturesId",
                        column: x => x.FixturesId,
                        principalTable: "Fixtures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventFixture_FixturesId",
                table: "EventFixture",
                column: "FixturesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventFixture");

            migrationBuilder.AddColumn<int>(
                name: "FixtureId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_FixtureId",
                table: "Events",
                column: "FixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Fixtures_FixtureId",
                table: "Events",
                column: "FixtureId",
                principalTable: "Fixtures",
                principalColumn: "Id");
        }
    }
}
