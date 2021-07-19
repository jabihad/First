using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddedTitleUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleUrl",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleUrl",
                table: "Posts");
        }
    }
}
