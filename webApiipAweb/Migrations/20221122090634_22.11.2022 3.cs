using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class _221120223 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "answear",
                table: "TaskWithOpenAnsws");

            migrationBuilder.DropColumn(
                name: "totalMark",
                table: "TaskWithOpenAnsws");

            migrationBuilder.AddColumn<double>(
                name: "mark",
                table: "TaskWithOpenAnswsExecutions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "mark",
                table: "TaskWithClosedAnswsExecutions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "mark",
                table: "AnswearOnTaskOpen",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mark",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropColumn(
                name: "mark",
                table: "TaskWithClosedAnswsExecutions");

            migrationBuilder.AddColumn<string>(
                name: "answear",
                table: "TaskWithOpenAnsws",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "totalMark",
                table: "TaskWithOpenAnsws",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "mark",
                table: "AnswearOnTaskOpen",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
