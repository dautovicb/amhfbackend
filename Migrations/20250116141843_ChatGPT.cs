using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealth.Migrations
{
    /// <inheritdoc />
    public partial class ChatGPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "postCount",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "postCount",
                table: "Categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
