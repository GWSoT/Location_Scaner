using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class HUbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Geolocation_MeetingLocationId",
                table: "Meeting");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityDate",
                table: "Profile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SignalRHubContextId",
                table: "Profile",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserStatus",
                table: "Profile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MeetingLocationId",
                table: "Meeting",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_Geolocation_MeetingLocationId",
                table: "Meeting",
                column: "MeetingLocationId",
                principalTable: "Geolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Geolocation_MeetingLocationId",
                table: "Meeting");

            migrationBuilder.DropColumn(
                name: "LastActivityDate",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "SignalRHubContextId",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "Profile");

            migrationBuilder.AlterColumn<int>(
                name: "MeetingLocationId",
                table: "Meeting",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_Geolocation_MeetingLocationId",
                table: "Meeting",
                column: "MeetingLocationId",
                principalTable: "Geolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
