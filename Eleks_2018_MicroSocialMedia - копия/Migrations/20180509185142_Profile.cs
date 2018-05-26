using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class Profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeolocationId",
                table: "Profile",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Geolocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(nullable: false),
                    Longtitude = table.Column<double>(nullable: false),
                    ProfileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geolocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Geolocation_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profile_GeolocationId",
                table: "Profile",
                column: "GeolocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Geolocation_ProfileId",
                table: "Geolocation",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_Geolocation_GeolocationId",
                table: "Profile",
                column: "GeolocationId",
                principalTable: "Geolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_Geolocation_GeolocationId",
                table: "Profile");

            migrationBuilder.DropTable(
                name: "Geolocation");

            migrationBuilder.DropIndex(
                name: "IX_Profile_GeolocationId",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "GeolocationId",
                table: "Profile");
        }
    }
}
