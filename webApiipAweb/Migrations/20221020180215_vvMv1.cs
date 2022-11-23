using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class vvMv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestTaskExecutions_AnswearOnTasks_idAnswearOnTask",
                table: "TestTaskExecutions");

            migrationBuilder.AlterColumn<int>(
                name: "idAnswearOnTask",
                table: "TestTaskExecutions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TestTaskExecutions_AnswearOnTasks_idAnswearOnTask",
                table: "TestTaskExecutions",
                column: "idAnswearOnTask",
                principalTable: "AnswearOnTasks",
                principalColumn: "idAnswearOnTask",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestTaskExecutions_AnswearOnTasks_idAnswearOnTask",
                table: "TestTaskExecutions");

            migrationBuilder.AlterColumn<int>(
                name: "idAnswearOnTask",
                table: "TestTaskExecutions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TestTaskExecutions_AnswearOnTasks_idAnswearOnTask",
                table: "TestTaskExecutions",
                column: "idAnswearOnTask",
                principalTable: "AnswearOnTasks",
                principalColumn: "idAnswearOnTask",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
