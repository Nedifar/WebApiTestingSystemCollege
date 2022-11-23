using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class addmigrationEditTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswearOnTasks_TestTasks_idTestTask",
                table: "AnswearOnTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_idTaskWithOpenAnsws",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropIndex(
                name: "IX_TaskWithOpenAnswsExecutions_idTaskWithOpenAnsws",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "TaskWithOpenAnswsExecutions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "idTaskWithOpenAnsws",
                table: "TaskWithOpenAnswsExecutions",
                newName: "idTask");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TaskWithOpenAnswsExecutions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswearResult",
                table: "TaskWithOpenAnswsExecutions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskWithOpenAnswidTaskWithOpenAnsw",
                table: "TaskWithOpenAnswsExecutions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypesTask",
                table: "TaskWithOpenAnsws",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "numericInPack",
                table: "TaskWithOpenAnsws",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "idTestTask",
                table: "AnswearOnTasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "idTask",
                table: "AnswearOnTasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskWithClosedAnsw",
                columns: table => new
                {
                    idTaskWithOpenAnsw = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    textQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    theme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numericInPack = table.Column<int>(type: "int", nullable: false),
                    TypesTask = table.Column<int>(type: "int", nullable: false),
                    idTestPack = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskWithClosedAnsw", x => x.idTaskWithOpenAnsw);
                    table.ForeignKey(
                        name: "FK_TaskWithClosedAnsw_TestPacks_idTestPack",
                        column: x => x.idTestPack,
                        principalTable: "TestPacks",
                        principalColumn: "idTestPack",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskWithClosedAnswsExecutions",
                columns: table => new
                {
                    idTaskWithOpenAnswsExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idAnswearOnTask = table.Column<int>(type: "int", nullable: true),
                    idTask = table.Column<int>(type: "int", nullable: true),
                    TaskWithClosedAnswidTaskWithOpenAnsw = table.Column<int>(type: "int", nullable: true),
                    idTestPackExecution = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    timeExecutionInSecond = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskWithClosedAnswsExecutions", x => x.idTaskWithOpenAnswsExecution);
                    table.ForeignKey(
                        name: "FK_TaskWithClosedAnswsExecutions_AnswearOnTasks_idAnswearOnTask",
                        column: x => x.idAnswearOnTask,
                        principalTable: "AnswearOnTasks",
                        principalColumn: "idAnswearOnTask",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskWithClosedAnswsExecutions_TaskWithClosedAnsw_TaskWithClosedAnswidTaskWithOpenAnsw",
                        column: x => x.TaskWithClosedAnswidTaskWithOpenAnsw,
                        principalTable: "TaskWithClosedAnsw",
                        principalColumn: "idTaskWithOpenAnsw",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskWithClosedAnswsExecutions_TestPackExecutions_idTestPackExecution",
                        column: x => x.idTestPackExecution,
                        principalTable: "TestPackExecutions",
                        principalColumn: "idTestPackExecution",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithOpenAnswsExecutions_TaskWithOpenAnswidTaskWithOpenAnsw",
                table: "TaskWithOpenAnswsExecutions",
                column: "TaskWithOpenAnswidTaskWithOpenAnsw");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions",
                column: "TaskWithClosedAnswidTaskWithOpenAnsw");

            migrationBuilder.CreateIndex(
                name: "IX_AnswearOnTasks_idTask",
                table: "AnswearOnTasks",
                column: "idTask");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithClosedAnsw_idTestPack",
                table: "TaskWithClosedAnsw",
                column: "idTestPack");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithClosedAnswsExecutions_idAnswearOnTask",
                table: "TaskWithClosedAnswsExecutions",
                column: "idAnswearOnTask");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithClosedAnswsExecutions_idTestPackExecution",
                table: "TaskWithClosedAnswsExecutions",
                column: "idTestPackExecution");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithClosedAnswsExecutions_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "TaskWithClosedAnswsExecutions",
                column: "TaskWithClosedAnswidTaskWithOpenAnsw");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswearOnTasks_TaskWithClosedAnsw_idTask",
                table: "AnswearOnTasks",
                column: "idTask",
                principalTable: "TaskWithClosedAnsw",
                principalColumn: "idTaskWithOpenAnsw",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswearOnTasks_TestTasks_idTestTask",
                table: "AnswearOnTasks",
                column: "idTestTask",
                principalTable: "TestTasks",
                principalColumn: "idTestTask",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_TaskWithClosedAnsw_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions",
                column: "TaskWithClosedAnswidTaskWithOpenAnsw",
                principalTable: "TaskWithClosedAnsw",
                principalColumn: "idTaskWithOpenAnsw",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_TaskWithOpenAnswidTaskWithOpenAnsw",
                table: "TaskWithOpenAnswsExecutions",
                column: "TaskWithOpenAnswidTaskWithOpenAnsw",
                principalTable: "TaskWithOpenAnsws",
                principalColumn: "idTaskWithOpenAnsw",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswearOnTasks_TaskWithClosedAnsw_idTask",
                table: "AnswearOnTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswearOnTasks_TestTasks_idTestTask",
                table: "AnswearOnTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_TaskWithClosedAnsw_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_TaskWithOpenAnswidTaskWithOpenAnsw",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropTable(
                name: "TaskWithClosedAnswsExecutions");

            migrationBuilder.DropTable(
                name: "TaskWithClosedAnsw");

            migrationBuilder.DropIndex(
                name: "IX_TaskWithOpenAnswsExecutions_TaskWithOpenAnswidTaskWithOpenAnsw",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions");

            migrationBuilder.DropIndex(
                name: "IX_AnswearOnTasks_idTask",
                table: "AnswearOnTasks");

            migrationBuilder.DropColumn(
                name: "AnswearResult",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropColumn(
                name: "TaskWithOpenAnswidTaskWithOpenAnsw",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropColumn(
                name: "TypesTask",
                table: "TaskWithOpenAnsws");

            migrationBuilder.DropColumn(
                name: "numericInPack",
                table: "TaskWithOpenAnsws");

            migrationBuilder.DropColumn(
                name: "TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "idTask",
                table: "AnswearOnTasks");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TaskWithOpenAnswsExecutions",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "idTask",
                table: "TaskWithOpenAnswsExecutions",
                newName: "idTaskWithOpenAnsws");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "TaskWithOpenAnswsExecutions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "idTestTask",
                table: "AnswearOnTasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithOpenAnswsExecutions_idTaskWithOpenAnsws",
                table: "TaskWithOpenAnswsExecutions",
                column: "idTaskWithOpenAnsws");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswearOnTasks_TestTasks_idTestTask",
                table: "AnswearOnTasks",
                column: "idTestTask",
                principalTable: "TestTasks",
                principalColumn: "idTestTask",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_idTaskWithOpenAnsws",
                table: "TaskWithOpenAnswsExecutions",
                column: "idTaskWithOpenAnsws",
                principalTable: "TaskWithOpenAnsws",
                principalColumn: "idTaskWithOpenAnsw",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
