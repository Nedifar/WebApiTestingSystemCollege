using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class SessionModified1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionChapterExecutions",
                columns: table => new
                {
                    idSessionChapterExecution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    beginDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idChild = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    idChapter = table.Column<int>(type: "int", nullable: false),
                    activeSession = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionChapterExecutions", x => x.idSessionChapterExecution);
                    table.ForeignKey(
                        name: "FK_SessionChapterExecutions_AspNetUsers_idChild",
                        column: x => x.idChild,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SessionChapterExecutions_Chapters_idChapter",
                        column: x => x.idChapter,
                        principalTable: "Chapters",
                        principalColumn: "idChapter",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionProgresses",
                columns: table => new
                {
                    idSessionProgress = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taskNumber = table.Column<int>(type: "int", nullable: false),
                    idSessionChapterExecution = table.Column<int>(type: "int", nullable: false),
                    StatusTaskExecution = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionProgresses", x => x.idSessionProgress);
                    table.ForeignKey(
                        name: "FK_SessionProgresses_SessionChapterExecutions_idSessionChapterExecution",
                        column: x => x.idSessionChapterExecution,
                        principalTable: "SessionChapterExecutions",
                        principalColumn: "idSessionChapterExecution",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionChapterExecutions_idChapter",
                table: "SessionChapterExecutions",
                column: "idChapter");

            migrationBuilder.CreateIndex(
                name: "IX_SessionChapterExecutions_idChild",
                table: "SessionChapterExecutions",
                column: "idChild");

            migrationBuilder.CreateIndex(
                name: "IX_SessionProgresses_idSessionChapterExecution",
                table: "SessionProgresses",
                column: "idSessionChapterExecution");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionProgresses");

            migrationBuilder.DropTable(
                name: "SessionChapterExecutions");
        }
    }
}
