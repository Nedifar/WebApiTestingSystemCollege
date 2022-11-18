using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class MainPackOtherPackupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "TestPacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isIncreasedComplexity",
                table: "TaskWithOpenAnsws",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isIncreasedComplexity",
                table: "TaskWithClosedAnsw",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "TestPacks");

            migrationBuilder.DropColumn(
                name: "isIncreasedComplexity",
                table: "TaskWithOpenAnsws");

            migrationBuilder.DropColumn(
                name: "isIncreasedComplexity",
                table: "TaskWithClosedAnsw");
        }
    }
}
