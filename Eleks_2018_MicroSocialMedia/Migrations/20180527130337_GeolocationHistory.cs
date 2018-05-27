using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class GeolocationHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Profile_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Device_Profile_ProfileId",
                table: "Device");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Profile_RequestedById",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Profile_RequestedToId",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Geolocation_Profile_ProfileId",
                table: "Geolocation");

            migrationBuilder.DropForeignKey(
                name: "FK_LastGeolocation_Profile_ProfileId",
                table: "LastGeolocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Profile_ProfileId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Profile_ProfileId",
                table: "Meeting");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Profile_MessageFromId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageGroupProfile_Profile_ProfileId",
                table: "MessageGroupProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Profile_ProfileId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Profile_ProfileId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Profile_LastGeolocation_LastGeolocationId",
                table: "Profile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profile",
                table: "Profile");

            migrationBuilder.RenameTable(
                name: "Profile",
                newName: "Profiles");

            migrationBuilder.RenameIndex(
                name: "IX_Profile_LastGeolocationId",
                table: "Profiles",
                newName: "IX_Profiles_LastGeolocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GeolocationHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GeolocationId = table.Column<int>(nullable: true),
                    ProfileId = table.Column<int>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeolocationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeolocationHistory_Geolocation_GeolocationId",
                        column: x => x.GeolocationId,
                        principalTable: "Geolocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeolocationHistory_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeolocationHistory_GeolocationId",
                table: "GeolocationHistory",
                column: "GeolocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeolocationHistory_ProfileId",
                table: "GeolocationHistory",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Profiles_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Device_Profiles_ProfileId",
                table: "Device",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Profiles_RequestedById",
                table: "Friend",
                column: "RequestedById",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Profiles_RequestedToId",
                table: "Friend",
                column: "RequestedToId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Geolocation_Profiles_ProfileId",
                table: "Geolocation",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LastGeolocation_Profiles_ProfileId",
                table: "LastGeolocation",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Profiles_ProfileId",
                table: "Like",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_Profiles_ProfileId",
                table: "Meeting",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Profiles_MessageFromId",
                table: "Message",
                column: "MessageFromId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageGroupProfile_Profiles_ProfileId",
                table: "MessageGroupProfile",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Profiles_ProfileId",
                table: "Notification",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Profiles_ProfileId",
                table: "Post",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_LastGeolocation_LastGeolocationId",
                table: "Profiles",
                column: "LastGeolocationId",
                principalTable: "LastGeolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Profiles_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Device_Profiles_ProfileId",
                table: "Device");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Profiles_RequestedById",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Profiles_RequestedToId",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Geolocation_Profiles_ProfileId",
                table: "Geolocation");

            migrationBuilder.DropForeignKey(
                name: "FK_LastGeolocation_Profiles_ProfileId",
                table: "LastGeolocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Profiles_ProfileId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Profiles_ProfileId",
                table: "Meeting");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Profiles_MessageFromId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageGroupProfile_Profiles_ProfileId",
                table: "MessageGroupProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Profiles_ProfileId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Profiles_ProfileId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_LastGeolocation_LastGeolocationId",
                table: "Profiles");

            migrationBuilder.DropTable(
                name: "GeolocationHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles");

            migrationBuilder.RenameTable(
                name: "Profiles",
                newName: "Profile");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_LastGeolocationId",
                table: "Profile",
                newName: "IX_Profile_LastGeolocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profile",
                table: "Profile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Profile_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Device_Profile_ProfileId",
                table: "Device",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Geolocation_Profile_ProfileId",
                table: "Geolocation",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LastGeolocation_Profile_ProfileId",
                table: "LastGeolocation",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Profile_ProfileId",
                table: "Like",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_Profile_ProfileId",
                table: "Meeting",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Profile_MessageFromId",
                table: "Message",
                column: "MessageFromId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageGroupProfile_Profile_ProfileId",
                table: "MessageGroupProfile",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Profile_ProfileId",
                table: "Notification",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Profile_ProfileId",
                table: "Post",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_LastGeolocation_LastGeolocationId",
                table: "Profile",
                column: "LastGeolocationId",
                principalTable: "LastGeolocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
