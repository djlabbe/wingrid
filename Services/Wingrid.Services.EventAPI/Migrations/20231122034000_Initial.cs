using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.EventAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Nickname = table.Column<string>(type: "text", nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    ShortDisplayName = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    AlternateColor = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    IsAllStar = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    SeasonType = table.Column<int>(type: "integer", nullable: true),
                    Season = table.Column<int>(type: "integer", nullable: true),
                    SeasonSlug = table.Column<string>(type: "text", nullable: true),
                    Week = table.Column<int>(type: "integer", nullable: true),
                    Attendance = table.Column<int>(type: "integer", nullable: true),
                    NeutralSite = table.Column<bool>(type: "boolean", nullable: true),
                    TimeValid = table.Column<bool>(type: "boolean", nullable: true),
                    ConferenceCompetition = table.Column<bool>(type: "boolean", nullable: true),
                    PlayByPlayAvailable = table.Column<bool>(type: "boolean", nullable: true),
                    Recent = table.Column<bool>(type: "boolean", nullable: true),
                    HomeWinner = table.Column<bool>(type: "boolean", nullable: true),
                    HomeTeamId = table.Column<string>(type: "text", nullable: true),
                    HomeScore = table.Column<string>(type: "text", nullable: true),
                    AwayWinner = table.Column<bool>(type: "boolean", nullable: true),
                    AwayTeamId = table.Column<string>(type: "text", nullable: true),
                    AwayScore = table.Column<string>(type: "text", nullable: true),
                    DisplayClock = table.Column<string>(type: "text", nullable: true),
                    Period = table.Column<int>(type: "integer", nullable: true),
                    StatusId = table.Column<string>(type: "text", nullable: true),
                    StatusName = table.Column<string>(type: "text", nullable: true),
                    StatusState = table.Column<string>(type: "text", nullable: true),
                    StatusCompleted = table.Column<bool>(type: "boolean", nullable: true),
                    StatusDescription = table.Column<string>(type: "text", nullable: true),
                    StatusDetail = table.Column<string>(type: "text", nullable: true),
                    StatusShortDetail = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_AwayTeamId",
                table: "Events",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_HomeTeamId",
                table: "Events",
                column: "HomeTeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
