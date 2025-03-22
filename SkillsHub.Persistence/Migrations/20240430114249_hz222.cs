using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class hz222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
            name: "ParentId",
            table: "BaseUserInfo",
            type: "uniqueidentifier",
            nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserInfo_ParentId",
                table: "BaseUserInfo",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseUserInfo_BaseUserInfo_ParentId",
                table: "BaseUserInfo",
                column: "ParentId",
                principalTable: "BaseUserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
        name: "FK_BaseUserInfo_BaseUserInfo_ParentId",
        table: "BaseUserInfo");

            migrationBuilder.DropIndex(
                name: "IX_BaseUserInfo_ParentId",
                table: "BaseUserInfo");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "BaseUserInfo");
        }
    }
}
