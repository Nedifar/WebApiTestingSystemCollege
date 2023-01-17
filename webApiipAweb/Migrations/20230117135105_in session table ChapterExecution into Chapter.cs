using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class insessiontableChapterExecutionintoChapter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionChapterExecutions_Chapters_idChapter",
                table: "SessionChapterExecutions");

            migrationBuilder.RenameColumn(
                name: "idChapter",
                table: "SessionChapterExecutions",
                newName: "idChapterExecution");

            migrationBuilder.RenameIndex(
                name: "IX_SessionChapterExecutions_idChapter",
                table: "SessionChapterExecutions",
                newName: "IX_SessionChapterExecutions_idChapterExecution");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChapterExecutions_ChapterExecutions_idChapterExecution",
                table: "SessionChapterExecutions",
                column: "idChapterExecution",
                principalTable: "ChapterExecutions",
                principalColumn: "idChapterExecution",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionChapterExecutions_ChapterExecutions_idChapterExecution",
                table: "SessionChapterExecutions");

            migrationBuilder.RenameColumn(
                name: "idChapterExecution",
                table: "SessionChapterExecutions",
                newName: "idChapter");

            migrationBuilder.RenameIndex(
                name: "IX_SessionChapterExecutions_idChapterExecution",
                table: "SessionChapterExecutions",
                newName: "IX_SessionChapterExecutions_idChapter");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChapterExecutions_Chapters_idChapter",
                table: "SessionChapterExecutions",
                column: "idChapter",
                principalTable: "Chapters",
                principalColumn: "idChapter",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
