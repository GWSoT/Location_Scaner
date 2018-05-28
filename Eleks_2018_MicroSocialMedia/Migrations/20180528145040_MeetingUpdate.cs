using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class MeetingUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Geolocation_MeetingLocationId",
                table: "Meeting");

            migrationBuilder.DropIndex(
                name: "IX_Meeting_MeetingLocationId",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "MeetingLocationId",
                table: "Meeting");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Meeting",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Meeting",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Meeting");

            migrationBuilder.AddColumn<int>(
                name: "MeetingLocationId",
                table: "Meeting",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meeting_MeetingLocationId",
                table: "Meeting",
                column: "MeetingLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_Geolocation_MeetingLocationId",
                table: "Meeting",
                column: "MeetingLocationId",
                principalTable: "Geolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
