using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class _22112022second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "totalMark",
                table: "TaskWithOpenAnsws",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AnswearOnTaskOpen",
                columns: table => new
                {
                    idAnswearOnTaskOpen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    answear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mark = table.Column<int>(type: "int", nullable: false),
                    idTask = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswearOnTaskOpen", x => x.idAnswearOnTaskOpen);
                    table.ForeignKey(
                        name: "FK_AnswearOnTaskOpen_TaskWithOpenAnsws_idTask",
                        column: x => x.idTask,
                        principalTable: "TaskWithOpenAnsws",
                        principalColumn: "idTask",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswearOnTaskOpen_idTask",
                table: "AnswearOnTaskOpen",
                column: "idTask");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswearOnTaskOpen");

            migrationBuilder.DropColumn(
                name: "totalMark",
                table: "TaskWithOpenAnsws");
        }
    }
}
