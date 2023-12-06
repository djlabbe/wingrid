﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.Services.FixtureAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEntryUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Entries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Entries");
        }
    }
}
