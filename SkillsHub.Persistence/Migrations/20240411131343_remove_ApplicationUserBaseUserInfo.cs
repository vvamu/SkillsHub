using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeApplicationUserBaseUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBaseUserInfo_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBaseUserInfo_BaseUserInfo_BaseUserInfoId",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserBaseUserInfo",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserBaseUserInfo_ApplicationUserId",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropColumn(
                name: "IsBase",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.RenameColumn(
                name: "BaseUserInfoId",
                table: "ApplicationUserBaseUserInfo",
                newName: "ConnectedUsersInfoId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "ApplicationUserBaseUserInfo",
                newName: "ApplicationUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserBaseUserInfo_BaseUserInfoId",
                table: "ApplicationUserBaseUserInfo",
                newName: "IX_ApplicationUserBaseUserInfo_ConnectedUsersInfoId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "BaseUserInfo",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsBase",
                table: "BaseUserInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserBaseUserInfo",
                table: "ApplicationUserBaseUserInfo",
                columns: new[] { "ApplicationUsersId", "ConnectedUsersInfoId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBaseUserInfo_AspNetUsers_ApplicationUsersId",
                table: "ApplicationUserBaseUserInfo",
                column: "ApplicationUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBaseUserInfo_BaseUserInfo_ConnectedUsersInfoId",
                table: "ApplicationUserBaseUserInfo",
                column: "ConnectedUsersInfoId",
                principalTable: "BaseUserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBaseUserInfo_AspNetUsers_ApplicationUsersId",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserBaseUserInfo_BaseUserInfo_ConnectedUsersInfoId",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserBaseUserInfo",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropColumn(
                name: "IsBase",
                table: "BaseUserInfo");

            migrationBuilder.RenameColumn(
                name: "ConnectedUsersInfoId",
                table: "ApplicationUserBaseUserInfo",
                newName: "BaseUserInfoId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUsersId",
                table: "ApplicationUserBaseUserInfo",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserBaseUserInfo_ConnectedUsersInfoId",
                table: "ApplicationUserBaseUserInfo",
                newName: "IX_ApplicationUserBaseUserInfo_BaseUserInfoId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "BaseUserInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ApplicationUserBaseUserInfo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ApplicationUserBaseUserInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsBase",
                table: "ApplicationUserBaseUserInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationUserBaseUserInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserBaseUserInfo",
                table: "ApplicationUserBaseUserInfo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserBaseUserInfo_ApplicationUserId",
                table: "ApplicationUserBaseUserInfo",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBaseUserInfo_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserBaseUserInfo",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserBaseUserInfo_BaseUserInfo_BaseUserInfoId",
                table: "ApplicationUserBaseUserInfo",
                column: "BaseUserInfoId",
                principalTable: "BaseUserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
