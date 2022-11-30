using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class _301120221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TheoreticalMaterialResource",
                columns: table => new
                {
                    idTheoreticalMaterialResource = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idTheoreticalMaterial = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheoreticalMaterialResource", x => x.idTheoreticalMaterialResource);
                    table.ForeignKey(
                        name: "FK_TheoreticalMaterialResource_TheoreticalMaterial_idTheoreticalMaterial",
                        column: x => x.idTheoreticalMaterial,
                        principalTable: "TheoreticalMaterial",
                        principalColumn: "idTheoreticalMaterial",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TheoreticalMaterialResource_idTheoreticalMaterial",
                table: "TheoreticalMaterialResource",
                column: "idTheoreticalMaterial");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TheoreticalMaterialResource");
        }
    }
}
