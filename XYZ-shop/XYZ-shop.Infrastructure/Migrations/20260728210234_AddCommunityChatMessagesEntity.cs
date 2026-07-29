using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XYZ_shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityChatMessagesEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameReviewEntity_Games_GameId",
                table: "GameReviewEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_GameReviewEntity_Users_AuthorId",
                table: "GameReviewEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameReviewEntity",
                table: "GameReviewEntity");

            migrationBuilder.RenameTable(
                name: "GameReviewEntity",
                newName: "GameReviews");

            migrationBuilder.RenameIndex(
                name: "IX_GameReviewEntity_GameId",
                table: "GameReviews",
                newName: "IX_GameReviews_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameReviewEntity_AuthorId",
                table: "GameReviews",
                newName: "IX_GameReviews_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameReviews",
                table: "GameReviews",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CommunityChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityChatMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityChatMessages_UserId",
                table: "CommunityChatMessages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameReviews_Games_GameId",
                table: "GameReviews",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameReviews_Users_AuthorId",
                table: "GameReviews",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameReviews_Games_GameId",
                table: "GameReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_GameReviews_Users_AuthorId",
                table: "GameReviews");

            migrationBuilder.DropTable(
                name: "CommunityChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameReviews",
                table: "GameReviews");

            migrationBuilder.RenameTable(
                name: "GameReviews",
                newName: "GameReviewEntity");

            migrationBuilder.RenameIndex(
                name: "IX_GameReviews_GameId",
                table: "GameReviewEntity",
                newName: "IX_GameReviewEntity_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameReviews_AuthorId",
                table: "GameReviewEntity",
                newName: "IX_GameReviewEntity_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameReviewEntity",
                table: "GameReviewEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameReviewEntity_Games_GameId",
                table: "GameReviewEntity",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameReviewEntity_Users_AuthorId",
                table: "GameReviewEntity",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
