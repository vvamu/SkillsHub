using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class hz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "ApplicationUserBaseUserInfo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BaseUserInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsBase = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApplicationUserBaseUserInfo", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApplicationUserBaseUserInfo_AspNetUsers_ApplicationUserId",
                    column: x => x.ApplicationUserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ApplicationUserBaseUserInfo_BaseUserInfo_BaseUserInfoId",
                    column: x => x.BaseUserInfoId,
                    principalTable: "BaseUserInfo",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
         name: "ApplicationUserBaseUserInfo");
        }
    }
}
