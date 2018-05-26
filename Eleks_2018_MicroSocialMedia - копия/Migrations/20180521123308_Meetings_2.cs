using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class Meetings_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_Geolocation_LastGeolocationId",
                table: "Profile");

            migrationBuilder.CreateTable(
                name: "LastGeolocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastGeolocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LastGeolocation_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LastGeolocation_ProfileId",
                table: "LastGeolocation",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_LastGeolocation_LastGeolocationId",
                table: "Profile",
                column: "LastGeolocationId",
                principalTable: "LastGeolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_LastGeolocation_LastGeolocationId",
                table: "Profile");

            migrationBuilder.DropTable(
                name: "LastGeolocation");

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_Geolocation_LastGeolocationId",
                table: "Profile",
                column: "LastGeolocationId",
                principalTable: "Geolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
