using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class Meetings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeetingId",
                table: "Friend",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Meeting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    ProfileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meeting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meeting_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friend_MeetingId",
                table: "Friend",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Meeting_ProfileId",
                table: "Meeting",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Meeting_MeetingId",
                table: "Friend",
                column: "MeetingId",
                principalTable: "Meeting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Meeting_MeetingId",
                table: "Friend");

            migrationBuilder.DropTable(
                name: "Meeting");

            migrationBuilder.DropIndex(
                name: "IX_Friend_MeetingId",
                table: "Friend");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "Friend");
        }
    }
}
