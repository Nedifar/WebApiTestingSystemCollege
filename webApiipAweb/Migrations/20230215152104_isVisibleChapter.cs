using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class isVisibleChapter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isVisible",
                table: "Chapters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isVisible",
                table: "Chapters");
        }
    }
}
