using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class lockedTimeinTaskExecutionParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "lockedTime",
                table: "TaskWithOpenAnswsExecutions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "lockedTime",
                table: "TaskWithClosedAnswsExecutions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "TheorySessions",
                columns: table => new
                {
                    idTheorySession = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    beginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    idChild = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    idChapterExecution = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheorySessions", x => x.idTheorySession);
                    table.ForeignKey(
                        name: "FK_TheorySessions_AspNetUsers_idChild",
                        column: x => x.idChild,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TheorySessions_ChapterExecutions_idChapterExecution",
                        column: x => x.idChapterExecution,
                        principalTable: "ChapterExecutions",
                        principalColumn: "idChapterExecution",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TheorySessions_idChapterExecution",
                table: "TheorySessions",
                column: "idChapterExecution");

            migrationBuilder.CreateIndex(
                name: "IX_TheorySessions_idChild",
                table: "TheorySessions",
                column: "idChild");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TheorySessions");

            migrationBuilder.DropColumn(
                name: "lockedTime",
                table: "TaskWithOpenAnswsExecutions");

            migrationBuilder.DropColumn(
                name: "lockedTime",
                table: "TaskWithClosedAnswsExecutions");
        }
    }
}
