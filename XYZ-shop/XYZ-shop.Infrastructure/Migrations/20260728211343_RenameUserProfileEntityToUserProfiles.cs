using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XYZ_shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserProfileEntityToUserProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfileEntity_UserProfileId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfileEntity",
                table: "UserProfileEntity");

            migrationBuilder.RenameTable(
                name: "UserProfileEntity",
                newName: "UserProfiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfiles_UserProfileId",
                table: "Users",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserProfiles_UserProfileId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "UserProfileEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfileEntity",
                table: "UserProfileEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserProfileEntity_UserProfileId",
                table: "Users",
                column: "UserProfileId",
                principalTable: "UserProfileEntity",
                principalColumn: "Id");
        }
    }
}
