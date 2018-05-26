using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class ProfileFriends_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Profile_FriendProfileId",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Profile_ProfileId",
                table: "Friend");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Friend",
                newName: "RequestedToId");

            migrationBuilder.RenameColumn(
                name: "FriendProfileId",
                table: "Friend",
                newName: "RequestedById");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_ProfileId",
                table: "Friend",
                newName: "IX_Friend_RequestedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_FriendProfileId",
                table: "Friend",
                newName: "IX_Friend_RequestedById");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Profile_RequestedById",
                table: "Friend",
                column: "RequestedById",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Profile_RequestedToId",
                table: "Friend",
                column: "RequestedToId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Profile_RequestedById",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Profile_RequestedToId",
                table: "Friend");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RequestedToId",
                table: "Friend",
                newName: "ProfileId");

            migrationBuilder.RenameColumn(
                name: "RequestedById",
                table: "Friend",
                newName: "FriendProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_RequestedToId",
                table: "Friend",
                newName: "IX_Friend_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_RequestedById",
                table: "Friend",
                newName: "IX_Friend_FriendProfileId");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Profile_FriendProfileId",
                table: "Friend",
                column: "FriendProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Profile_ProfileId",
                table: "Friend",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
