using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XYZ_shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEntityUserEntity",
                columns: table => new
                {
                    MyFriendsId = table.Column<int>(type: "int", nullable: false),
                    WhoIsMyFriendsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntityUserEntity", x => new { x.MyFriendsId, x.WhoIsMyFriendsId });
                    table.ForeignKey(
                        name: "FK_UserEntityUserEntity_Users_MyFriendsId",
                        column: x => x.MyFriendsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEntityUserEntity_Users_WhoIsMyFriendsId",
                        column: x => x.WhoIsMyFriendsId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_CreatedByUserId",
                table: "Games",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_ModifiedByUserId",
                table: "Games",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntityUserEntity_WhoIsMyFriendsId",
                table: "UserEntityUserEntity",
                column: "WhoIsMyFriendsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_CreatedByUserId",
                table: "Games",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_ModifiedByUserId",
                table: "Games",
                column: "ModifiedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_CreatedByUserId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_ModifiedByUserId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "UserEntityUserEntity");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Games_CreatedByUserId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ModifiedByUserId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Games");
        }
    }
}
