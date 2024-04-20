using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removemanytomanyApplicationUserBaseUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserBaseUserInfo");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "BaseUserInfo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserInfo_ApplicationUserId",
                table: "BaseUserInfo",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseUserInfo_AspNetUsers_ApplicationUserId",
                table: "BaseUserInfo",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseUserInfo_AspNetUsers_ApplicationUserId",
                table: "BaseUserInfo");

            migrationBuilder.DropIndex(
                name: "IX_BaseUserInfo_ApplicationUserId",
                table: "BaseUserInfo");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "BaseUserInfo");

            migrationBuilder.CreateTable(
                name: "ApplicationUserBaseUserInfo",
                columns: table => new
                {
                    ApplicationUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectedUsersInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserBaseUserInfo", x => new { x.ApplicationUsersId, x.ConnectedUsersInfoId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserBaseUserInfo_AspNetUsers_ApplicationUsersId",
                        column: x => x.ApplicationUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserBaseUserInfo_BaseUserInfo_ConnectedUsersInfoId",
                        column: x => x.ConnectedUsersInfoId,
                        principalTable: "BaseUserInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserBaseUserInfo_ConnectedUsersInfoId",
                table: "ApplicationUserBaseUserInfo",
                column: "ConnectedUsersInfoId");
        }
    }
}
