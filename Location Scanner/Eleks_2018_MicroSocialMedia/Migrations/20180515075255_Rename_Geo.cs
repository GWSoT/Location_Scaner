using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class Rename_Geo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Geolocation_Profile_ProfileId",
                table: "Geolocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Profile_Geolocation_GeolocationId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_GeolocationId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Geolocation_ProfileId",
                table: "Geolocation");

            migrationBuilder.RenameColumn(
                name: "Longtitude",
                table: "Geolocation",
                newName: "Longitude");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Geolocation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Geolocation_ProfileId",
                table: "Geolocation",
                column: "ProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Geolocation_Profile_ProfileId",
                table: "Geolocation",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Geolocation_Profile_ProfileId",
                table: "Geolocation");

            migrationBuilder.DropIndex(
                name: "IX_Geolocation_ProfileId",
                table: "Geolocation");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Geolocation",
                newName: "Longtitude");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Geolocation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Profile_GeolocationId",
                table: "Profile",
                column: "GeolocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Geolocation_ProfileId",
                table: "Geolocation",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Geolocation_Profile_ProfileId",
                table: "Geolocation",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_Geolocation_GeolocationId",
                table: "Profile",
                column: "GeolocationId",
                principalTable: "Geolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
