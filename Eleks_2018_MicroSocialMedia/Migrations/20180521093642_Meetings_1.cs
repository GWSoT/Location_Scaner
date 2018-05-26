using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class Meetings_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDistance",
                table: "Friend");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Meeting",
                newName: "MeetingTime");

            migrationBuilder.AddColumn<int>(
                name: "LastGeolocationId",
                table: "Profile",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MeetingLocationId",
                table: "Meeting",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Profile_LastGeolocationId",
                table: "Profile",
                column: "LastGeolocationId");

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_Geolocation_LastGeolocationId",
                table: "Profile",
                column: "LastGeolocationId",
                principalTable: "Geolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Geolocation_MeetingLocationId",
                table: "Meeting");

            migrationBuilder.DropForeignKey(
                name: "FK_Profile_Geolocation_LastGeolocationId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_LastGeolocationId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Meeting_MeetingLocationId",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "LastGeolocationId",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "MeetingLocationId",
                table: "Meeting");

            migrationBuilder.RenameColumn(
                name: "MeetingTime",
                table: "Meeting",
                newName: "DateTime");

            migrationBuilder.AddColumn<double>(
                name: "LastDistance",
                table: "Friend",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
