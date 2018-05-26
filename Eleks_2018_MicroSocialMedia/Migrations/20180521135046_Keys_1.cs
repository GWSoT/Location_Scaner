using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class Keys_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LastGeolocation_Profile_ProfileId",
                table: "LastGeolocation");

            migrationBuilder.DropIndex(
                name: "IX_Geolocation_ProfileId",
                table: "Geolocation");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "LastGeolocation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Geolocation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Geolocation_ProfileId",
                table: "Geolocation",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_LastGeolocation_Profile_ProfileId",
                table: "LastGeolocation",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LastGeolocation_Profile_ProfileId",
                table: "LastGeolocation");

            migrationBuilder.DropIndex(
                name: "IX_Geolocation_ProfileId",
                table: "Geolocation");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "LastGeolocation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
                name: "FK_LastGeolocation_Profile_ProfileId",
                table: "LastGeolocation",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
