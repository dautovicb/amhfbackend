using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealth.Migrations
{
    /// <inheritdoc />
    public partial class CategoryNameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "likes",
                table: "Posts",
                newName: "likeCount");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_name",
                table: "Categories",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_name",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "likeCount",
                table: "Posts",
                newName: "likes");
        }
    }
}
