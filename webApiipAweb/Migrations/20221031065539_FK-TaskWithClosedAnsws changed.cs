using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class FKTaskWithClosedAnswschanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithClosedAnswsExecutions_TaskWithClosedAnsw_TaskWithClosedAnswidTask",
                table: "TaskWithClosedAnswsExecutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_TaskWithOpenAnswidTask",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropIndex(
                name: "IX_TaskWithOpenAnswsExecutions_TaskWithOpenAnswidTask",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropIndex(
                name: "IX_TaskWithClosedAnswsExecutions_TaskWithClosedAnswidTask",
                table: "TaskWithClosedAnswsExecutions");

            migrationBuilder.DropColumn(
                name: "TaskWithOpenAnswidTask",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropColumn(
                name: "TaskWithClosedAnswidTask",
                table: "TaskWithClosedAnswsExecutions");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithOpenAnswsExecutions_idTask",
                table: "TaskWithOpenAnswsExecutions",
                column: "idTask");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithClosedAnswsExecutions_idTask",
                table: "TaskWithClosedAnswsExecutions",
                column: "idTask");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWithClosedAnswsExecutions_TaskWithClosedAnsw_idTask",
                table: "TaskWithClosedAnswsExecutions",
                column: "idTask",
                principalTable: "TaskWithClosedAnsw",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_idTask",
                table: "TaskWithOpenAnswsExecutions",
                column: "idTask",
                principalTable: "TaskWithOpenAnsws",
                principalColumn: "idTask",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithClosedAnswsExecutions_TaskWithClosedAnsw_idTask",
                table: "TaskWithClosedAnswsExecutions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskWithOpenAnswsExecutions_TaskWithOpenAnsws_idTask",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropIndex(
                name: "IX_TaskWithOpenAnswsExecutions_idTask",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropIndex(
                name: "IX_TaskWithClosedAnswsExecutions_idTask",
                table: "TaskWithClosedAnswsExecutions");

            migrationBuilder.AddColumn<int>(
                name: "TaskWithOpenAnswidTask",
                table: "TaskWithOpenAnswsExecutions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskWithClosedAnswidTask",
                table: "TaskWithClosedAnswsExecutions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithOpenAnswsExecutions_TaskWithOpenAnswidTask",
                table: "TaskWithOpenAnswsExecutions",
                column: "TaskWithOpenAnswidTask");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWithClosedAnswsExecutions_TaskWithClosedAnswidTask",
                table: "TaskWithClosedAnswsExecutions",
                column: "TaskWithClosedAnswidTask");

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
    }
}
