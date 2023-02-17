using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class idTaskSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_TaskWithOpenAnsws_idTaskWithOpenAnsw",
                table: "Solutions");

            migrationBuilder.RenameColumn(
                name: "idTaskWithOpenAnsw",
                table: "Solutions",
                newName: "idTask");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_idTaskWithOpenAnsw",
                table: "Solutions",
                newName: "IX_Solutions_idTask");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_TaskWithOpenAnsws_idTask",
                table: "Solutions",
                column: "idTask",
                principalTable: "TaskWithOpenAnsws",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_TaskWithOpenAnsws_idTask",
                table: "Solutions");

            migrationBuilder.RenameColumn(
                name: "idTask",
                table: "Solutions",
                newName: "idTaskWithOpenAnsw");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_idTask",
                table: "Solutions",
                newName: "IX_Solutions_idTaskWithOpenAnsw");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_TaskWithOpenAnsws_idTaskWithOpenAnsw",
                table: "Solutions",
                column: "idTaskWithOpenAnsw",
                principalTable: "TaskWithOpenAnsws",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
