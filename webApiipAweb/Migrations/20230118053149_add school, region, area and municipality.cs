using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class addschoolregionareaandmunicipality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idMunicipality",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "idSchool",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    idRegion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    regionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.idRegion);
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    idArea = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    areaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idRegion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.idArea);
                    table.ForeignKey(
                        name: "FK_Area_Region_idRegion",
                        column: x => x.idRegion,
                        principalTable: "Region",
                        principalColumn: "idRegion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Municipality",
                columns: table => new
                {
                    idMunicipality = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idArea = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipality", x => x.idMunicipality);
                    table.ForeignKey(
                        name: "FK_Municipality_Area_idArea",
                        column: x => x.idArea,
                        principalTable: "Area",
                        principalColumn: "idArea",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    idSchool = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    idMunicipality = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.idSchool);
                    table.ForeignKey(
                        name: "FK_School_Municipality_idMunicipality",
                        column: x => x.idMunicipality,
                        principalTable: "Municipality",
                        principalColumn: "idMunicipality",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_idMunicipality",
                table: "AspNetUsers",
                column: "idMunicipality");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_idSchool",
                table: "AspNetUsers",
                column: "idSchool");

            migrationBuilder.CreateIndex(
                name: "IX_Area_idRegion",
                table: "Area",
                column: "idRegion");

            migrationBuilder.CreateIndex(
                name: "IX_Municipality_idArea",
                table: "Municipality",
                column: "idArea");

            migrationBuilder.CreateIndex(
                name: "IX_School_idMunicipality",
                table: "School",
                column: "idMunicipality");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Municipality_idMunicipality",
                table: "AspNetUsers",
                column: "idMunicipality",
                principalTable: "Municipality",
                principalColumn: "idMunicipality",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_School_idSchool",
                table: "AspNetUsers",
                column: "idSchool",
                principalTable: "School",
                principalColumn: "idSchool",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Municipality_idMunicipality",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_School_idSchool",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropTable(
                name: "Municipality");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_idMunicipality",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_idSchool",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "idMunicipality",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "idSchool",
                table: "AspNetUsers");
        }
    }
}
