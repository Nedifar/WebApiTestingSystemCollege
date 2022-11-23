using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class _231120221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResultType",
                table: "TaskWithOpenAnsws",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "htmlModel",
                table: "TaskWithOpenAnsws",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultType",
                table: "TaskWithOpenAnsws");

            migrationBuilder.DropColumn(
                name: "htmlModel",
                table: "TaskWithOpenAnsws");
        }
    }
}
