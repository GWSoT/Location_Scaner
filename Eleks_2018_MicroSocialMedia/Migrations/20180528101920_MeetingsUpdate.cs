using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class MeetingsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Meeting_MeetingId",
                table: "Friend");

            migrationBuilder.DropIndex(
                name: "IX_Friend_MeetingId",
                table: "Friend");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "Friend");

            migrationBuilder.CreateTable(
                name: "MeetingProfile",
                columns: table => new
                {
                    MeetingId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingProfile", x => new { x.MeetingId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_MeetingProfile_Meeting_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingProfile_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingProfile_ProfileId",
                table: "MeetingProfile",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingProfile");

            migrationBuilder.AddColumn<int>(
                name: "MeetingId",
                table: "Friend",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friend_MeetingId",
                table: "Friend",
                column: "MeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Meeting_MeetingId",
                table: "Friend",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
