using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class identityTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_TaskWithClosedAnsw_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithClosedAnswsExecutions_TaskWithClosedAnsw_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "TaskWithClosedAnswsExecutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_TaskWithOpenAnswidTaskWithOpenAnsw",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.RenameColumn(
                name: "TaskWithOpenAnswidTaskWithOpenAnsw",
                table: "TaskWithOpenAnswsExecutions",
                newName: "TaskWithOpenAnswidTask");

            migrationBuilder.RenameColumn(
                name: "idTaskWithOpenAnswsExecution",
                table: "TaskWithOpenAnswsExecutions",
                newName: "idTaskExecution");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWithOpenAnswsExecutions_TaskWithOpenAnswidTaskWithOpenAnsw",
                table: "TaskWithOpenAnswsExecutions",
                newName: "IX_TaskWithOpenAnswsExecutions_TaskWithOpenAnswidTask");

            migrationBuilder.RenameColumn(
                name: "idTaskWithOpenAnsw",
                table: "TaskWithOpenAnsws",
                newName: "idTask");

            migrationBuilder.RenameColumn(
                name: "TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "TaskWithClosedAnswsExecutions",
                newName: "TaskWithClosedAnswidTask");

            migrationBuilder.RenameColumn(
                name: "idTaskWithOpenAnswsExecution",
                table: "TaskWithClosedAnswsExecutions",
                newName: "idTaskExecution");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWithClosedAnswsExecutions_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "TaskWithClosedAnswsExecutions",
                newName: "IX_TaskWithClosedAnswsExecutions_TaskWithClosedAnswidTask");

            migrationBuilder.RenameColumn(
                name: "idTaskWithOpenAnsw",
                table: "TaskWithClosedAnsw",
                newName: "idTask");

            migrationBuilder.RenameColumn(
                name: "TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions",
                newName: "TaskWithClosedAnswidTask");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions",
                newName: "IX_Solutions_TaskWithClosedAnswidTask");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_TaskWithClosedAnsw_TaskWithClosedAnswidTask",
                table: "Solutions",
                column: "TaskWithClosedAnswidTask",
                principalTable: "TaskWithClosedAnsw",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWithClosedAnswsExecutions_TaskWithClosedAnsw_TaskWithClosedAnswidTask",
                table: "TaskWithClosedAnswsExecutions",
                column: "TaskWithClosedAnswidTask",
                principalTable: "TaskWithClosedAnsw",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_TaskWithOpenAnswidTask",
                table: "TaskWithOpenAnswsExecutions",
                column: "TaskWithOpenAnswidTask",
                principalTable: "TaskWithOpenAnsws",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_TaskWithClosedAnsw_TaskWithClosedAnswidTask",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithClosedAnswsExecutions_TaskWithClosedAnsw_TaskWithClosedAnswidTask",
                table: "TaskWithClosedAnswsExecutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_TaskWithOpenAnswidTask",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.RenameColumn(
                name: "TaskWithOpenAnswidTask",
                table: "TaskWithOpenAnswsExecutions",
                newName: "TaskWithOpenAnswidTaskWithOpenAnsw");

            migrationBuilder.RenameColumn(
                name: "idTaskExecution",
                table: "TaskWithOpenAnswsExecutions",
                newName: "idTaskWithOpenAnswsExecution");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWithOpenAnswsExecutions_TaskWithOpenAnswidTask",
                table: "TaskWithOpenAnswsExecutions",
                newName: "IX_TaskWithOpenAnswsExecutions_TaskWithOpenAnswidTaskWithOpenAnsw");

            migrationBuilder.RenameColumn(
                name: "idTask",
                table: "TaskWithOpenAnsws",
                newName: "idTaskWithOpenAnsw");

            migrationBuilder.RenameColumn(
                name: "TaskWithClosedAnswidTask",
                table: "TaskWithClosedAnswsExecutions",
                newName: "TaskWithClosedAnswidTaskWithOpenAnsw");

            migrationBuilder.RenameColumn(
                name: "idTaskExecution",
                table: "TaskWithClosedAnswsExecutions",
                newName: "idTaskWithOpenAnswsExecution");

            migrationBuilder.RenameIndex(
                name: "IX_TaskWithClosedAnswsExecutions_TaskWithClosedAnswidTask",
                table: "TaskWithClosedAnswsExecutions",
                newName: "IX_TaskWithClosedAnswsExecutions_TaskWithClosedAnswidTaskWithOpenAnsw");

            migrationBuilder.RenameColumn(
                name: "idTask",
                table: "TaskWithClosedAnsw",
                newName: "idTaskWithOpenAnsw");

            migrationBuilder.RenameColumn(
                name: "TaskWithClosedAnswidTask",
                table: "Solutions",
                newName: "TaskWithClosedAnswidTaskWithOpenAnsw");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_TaskWithClosedAnswidTask",
                table: "Solutions",
                newName: "IX_Solutions_TaskWithClosedAnswidTaskWithOpenAnsw");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_TaskWithClosedAnsw_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "Solutions",
                column: "TaskWithClosedAnswidTaskWithOpenAnsw",
                principalTable: "TaskWithClosedAnsw",
                principalColumn: "idTaskWithOpenAnsw",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWithClosedAnswsExecutions_TaskWithClosedAnsw_TaskWithClosedAnswidTaskWithOpenAnsw",
                table: "TaskWithClosedAnswsExecutions",
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
    }
}
