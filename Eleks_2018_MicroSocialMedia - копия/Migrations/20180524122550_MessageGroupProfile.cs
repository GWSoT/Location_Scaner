using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Eleks_2018_MicroSocialMedia.Migrations
{
    public partial class MessageGroupProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MessageBody = table.Column<string>(nullable: true),
                    MessageDate = table.Column<DateTime>(nullable: false),
                    MessageFromId = table.Column<int>(nullable: true),
                    MessageGroupId = table.Column<Guid>(nullable: true),
                    MessageStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Profile_MessageFromId",
                        column: x => x.MessageFromId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_MessageGroup_MessageGroupId",
                        column: x => x.MessageGroupId,
                        principalTable: "MessageGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageGroupProfile",
                columns: table => new
                {
                    MessageGroupId = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageGroupProfile", x => new { x.MessageGroupId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_MessageGroupProfile_MessageGroup_MessageGroupId",
                        column: x => x.MessageGroupId,
                        principalTable: "MessageGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageGroupProfile_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_MessageFromId",
                table: "Message",
                column: "MessageFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_MessageGroupId",
                table: "Message",
                column: "MessageGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageGroupProfile_ProfileId",
                table: "MessageGroupProfile",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "MessageGroupProfile");

            migrationBuilder.DropTable(
                name: "MessageGroup");
        }
    }
}
