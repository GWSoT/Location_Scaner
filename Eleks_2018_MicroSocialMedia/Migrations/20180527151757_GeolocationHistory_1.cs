using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class GeolocationHistory_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeolocationHistory_Geolocation_GeolocationId",
                table: "GeolocationHistory");

            migrationBuilder.DropIndex(
                name: "IX_GeolocationHistory_GeolocationId",
                table: "GeolocationHistory");

            migrationBuilder.DropColumn(
                name: "GeolocationId",
                table: "GeolocationHistory");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "GeolocationHistory",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "GeolocationHistory",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "GeolocationHistory");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "GeolocationHistory");

            migrationBuilder.AddColumn<int>(
                name: "GeolocationId",
                table: "GeolocationHistory",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeolocationHistory_GeolocationId",
                table: "GeolocationHistory",
                column: "GeolocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeolocationHistory_Geolocation_GeolocationId",
                table: "GeolocationHistory",
                column: "GeolocationId",
                principalTable: "Geolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
