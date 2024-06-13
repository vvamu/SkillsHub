using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class renameNotificationUsertablecolumnnotificationMessageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUsers_NotificationMessages_NotificationId",
                table: "NotificationUsers");

            migrationBuilder.RenameColumn(
                name: "NotificationId",
                table: "NotificationUsers",
                newName: "NotificationMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationUsers_NotificationId",
                table: "NotificationUsers",
                newName: "IX_NotificationUsers_NotificationMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUsers_NotificationMessages_NotificationMessageId",
                table: "NotificationUsers",
                column: "NotificationMessageId",
                principalTable: "NotificationMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUsers_NotificationMessages_NotificationMessageId",
                table: "NotificationUsers");

            migrationBuilder.RenameColumn(
                name: "NotificationMessageId",
                table: "NotificationUsers",
                newName: "NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationUsers_NotificationMessageId",
                table: "NotificationUsers",
                newName: "IX_NotificationUsers_NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUsers_NotificationMessages_NotificationId",
                table: "NotificationUsers",
                column: "NotificationId",
                principalTable: "NotificationMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
