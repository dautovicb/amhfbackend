using Microsoft.EntityFrameworkCore.Migrations;

namespace MentalHealth.Migrations
{
    public partial class AddPostSearchIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create a combined index for title and content
            migrationBuilder.Sql(
                @"CREATE INDEX IX_Posts_TitleContent 
                  ON Posts (title, content)
                  INCLUDE (id, categoryId, likeCount)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IX_Posts_TitleContent ON Posts");
        }
    }
}