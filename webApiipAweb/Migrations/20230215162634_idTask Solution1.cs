using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class idTaskSolution1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_TaskWithClosedAnsw_TaskWithClosedAnswidTask",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_TaskWithOpenAnsws_idTask",
                table: "Solutions");

            migrationBuilder.RenameColumn(
                name: "idTask",
                table: "Solutions",
                newName: "idTaskWithOpenAnsw");

            migrationBuilder.RenameColumn(
                name: "TaskWithClosedAnswidTask",
                table: "Solutions",
                newName: "idTaskWithClosedAnsw");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_TaskWithClosedAnswidTask",
                table: "Solutions",
                newName: "IX_Solutions_idTaskWithClosedAnsw");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_idTask",
                table: "Solutions",
                newName: "IX_Solutions_idTaskWithOpenAnsw");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_TaskWithClosedAnsw_idTaskWithClosedAnsw",
                table: "Solutions",
                column: "idTaskWithClosedAnsw",
                principalTable: "TaskWithClosedAnsw",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_TaskWithOpenAnsws_idTaskWithOpenAnsw",
                table: "Solutions",
                column: "idTaskWithOpenAnsw",
                principalTable: "TaskWithOpenAnsws",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_TaskWithClosedAnsw_idTaskWithClosedAnsw",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_TaskWithOpenAnsws_idTaskWithOpenAnsw",
                table: "Solutions");

            migrationBuilder.RenameColumn(
                name: "idTaskWithOpenAnsw",
                table: "Solutions",
                newName: "idTask");

            migrationBuilder.RenameColumn(
                name: "idTaskWithClosedAnsw",
                table: "Solutions",
                newName: "TaskWithClosedAnswidTask");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_idTaskWithOpenAnsw",
                table: "Solutions",
                newName: "IX_Solutions_idTask");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_idTaskWithClosedAnsw",
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
                name: "FK_Solutions_TaskWithOpenAnsws_idTask",
                table: "Solutions",
                column: "idTask",
                principalTable: "TaskWithOpenAnsws",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
