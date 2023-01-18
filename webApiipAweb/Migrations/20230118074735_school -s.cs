using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class schools : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Area_Region_idRegion",
                table: "Area");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Municipality_idMunicipality",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_School_idSchool",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Municipality_Area_idArea",
                table: "Municipality");

            migrationBuilder.DropForeignKey(
                name: "FK_School_Municipality_idMunicipality",
                table: "School");

            migrationBuilder.DropPrimaryKey(
                name: "PK_School",
                table: "School");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Region",
                table: "Region");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Municipality",
                table: "Municipality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Area",
                table: "Area");

            migrationBuilder.RenameTable(
                name: "School",
                newName: "Schools");

            migrationBuilder.RenameTable(
                name: "Region",
                newName: "Regions");

            migrationBuilder.RenameTable(
                name: "Municipality",
                newName: "Municipalities");

            migrationBuilder.RenameTable(
                name: "Area",
                newName: "Areas");

            migrationBuilder.RenameIndex(
                name: "IX_School_idMunicipality",
                table: "Schools",
                newName: "IX_Schools_idMunicipality");

            migrationBuilder.RenameIndex(
                name: "IX_Municipality_idArea",
                table: "Municipalities",
                newName: "IX_Municipalities_idArea");

            migrationBuilder.RenameIndex(
                name: "IX_Area_idRegion",
                table: "Areas",
                newName: "IX_Areas_idRegion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schools",
                table: "Schools",
                column: "idSchool");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regions",
                table: "Regions",
                column: "idRegion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Municipalities",
                table: "Municipalities",
                column: "idMunicipality");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Areas",
                table: "Areas",
                column: "idArea");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Regions_idRegion",
                table: "Areas",
                column: "idRegion",
                principalTable: "Regions",
                principalColumn: "idRegion",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Municipalities_idMunicipality",
                table: "AspNetUsers",
                column: "idMunicipality",
                principalTable: "Municipalities",
                principalColumn: "idMunicipality",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Schools_idSchool",
                table: "AspNetUsers",
                column: "idSchool",
                principalTable: "Schools",
                principalColumn: "idSchool",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Municipalities_Areas_idArea",
                table: "Municipalities",
                column: "idArea",
                principalTable: "Areas",
                principalColumn: "idArea",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Municipalities_idMunicipality",
                table: "Schools",
                column: "idMunicipality",
                principalTable: "Municipalities",
                principalColumn: "idMunicipality",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Regions_idRegion",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Municipalities_idMunicipality",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Schools_idSchool",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Municipalities_Areas_idArea",
                table: "Municipalities");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Municipalities_idMunicipality",
                table: "Schools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schools",
                table: "Schools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regions",
                table: "Regions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Municipalities",
                table: "Municipalities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Areas",
                table: "Areas");

            migrationBuilder.RenameTable(
                name: "Schools",
                newName: "School");

            migrationBuilder.RenameTable(
                name: "Regions",
                newName: "Region");

            migrationBuilder.RenameTable(
                name: "Municipalities",
                newName: "Municipality");

            migrationBuilder.RenameTable(
                name: "Areas",
                newName: "Area");

            migrationBuilder.RenameIndex(
                name: "IX_Schools_idMunicipality",
                table: "School",
                newName: "IX_School_idMunicipality");

            migrationBuilder.RenameIndex(
                name: "IX_Municipalities_idArea",
                table: "Municipality",
                newName: "IX_Municipality_idArea");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_idRegion",
                table: "Area",
                newName: "IX_Area_idRegion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_School",
                table: "School",
                column: "idSchool");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Region",
                table: "Region",
                column: "idRegion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Municipality",
                table: "Municipality",
                column: "idMunicipality");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Area",
                table: "Area",
                column: "idArea");

            migrationBuilder.AddForeignKey(
                name: "FK_Area_Region_idRegion",
                table: "Area",
                column: "idRegion",
                principalTable: "Region",
                principalColumn: "idRegion",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Municipality_Area_idArea",
                table: "Municipality",
                column: "idArea",
                principalTable: "Area",
                principalColumn: "idArea",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_School_Municipality_idMunicipality",
                table: "School",
                column: "idMunicipality",
                principalTable: "Municipality",
                principalColumn: "idMunicipality",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
