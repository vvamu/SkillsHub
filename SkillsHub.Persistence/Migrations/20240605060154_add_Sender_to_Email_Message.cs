using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addSendertoEmailMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<Guid>(
                name: "SenderId",
                table: "EmailMessages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailMessages_SenderId",
                table: "EmailMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_AspNetUsers_SenderId",
                table: "EmailMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_AspNetUsers_SenderId",
                table: "EmailMessages");

            migrationBuilder.DropIndex(
                name: "IX_EmailMessages_SenderId",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "EmailMessages");

            migrationBuilder.AddColumn<DateTime>(
                name: "UserStartDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
