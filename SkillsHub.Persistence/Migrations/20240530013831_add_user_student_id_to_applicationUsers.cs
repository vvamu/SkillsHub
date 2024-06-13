using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class adduserstudentidtoapplicationUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            /*
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_ApplicationUserId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_AspNetUsers_ApplicationUserId",
                table: "Teachers");
*/
            /*
            migrationBuilder.AddForeignKey(
            name: "FK_Students_AspNetUsers_ApplicationUserId",
            table: "Students",
            column: "ApplicationUserId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
            name: "FK_Teachers_AspNetUsers_ApplicationUserId",
            table: "Teachers",
            column: "ApplicationUserId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

            */

            /*
            migrationBuilder.DropIndex(
                name: "IX_Teachers_ApplicationUserId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_ApplicationUserId",
                table: "Students");

            migrationBuilder.AddColumn<Guid>(
                name: "UserStudentId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserTeacherId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserStudentId",
                table: "AspNetUsers",
                column: "UserStudentId",
                unique: true,
                filter: "[UserStudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserTeacherId",
                table: "AspNetUsers",
                column: "UserTeacherId",
                unique: true,
                filter: "[UserTeacherId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Students_UserStudentId", 
                table: "AspNetUsers",
                column: "UserStudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teachers_UserTeacherId",
                table: "AspNetUsers",
                column: "UserTeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Students_UserStudentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teachers_UserTeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserStudentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserTeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserStudentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserTeacherId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ApplicationUserId",
                table: "Teachers",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ApplicationUserId",
                table: "Students",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_ApplicationUserId",
                table: "Students",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_AspNetUsers_ApplicationUserId",
                table: "Teachers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
