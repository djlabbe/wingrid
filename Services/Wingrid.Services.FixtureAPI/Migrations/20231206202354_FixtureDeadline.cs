using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.Services.FixtureAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixtureDeadline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locked",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "LockedAt",
                table: "Fixtures");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Fixtures",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Fixtures");

            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "Fixtures",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedAt",
                table: "Fixtures",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
