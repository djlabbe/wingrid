﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wingrid.Services.FixtureAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEventIdToEventEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventId",
                table: "EventEntry",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                table: "EventEntry");
        }
    }
}