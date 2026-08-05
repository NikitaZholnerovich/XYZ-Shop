using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XYZ_shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingFieldsToGameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Games",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositiveReviewsCount",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewsCount",
                table: "Games",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PositiveReviewsCount",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ReviewsCount",
                table: "Games");
        }
    }
}
