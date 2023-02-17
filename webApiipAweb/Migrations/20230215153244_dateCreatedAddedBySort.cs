using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace webApiipAweb.Migrations
{
    public partial class dateCreatedAddedBySort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "startDate",
                table: "TryingTestTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "dateCreated",
                table: "TaskWithOpenAnsws",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "dateCreated",
                table: "TaskWithClosedAnsw",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "startDate",
                table: "TryingTestTasks");

            migrationBuilder.DropColumn(
                name: "dateCreated",
                table: "TaskWithOpenAnsws");

            migrationBuilder.DropColumn(
                name: "dateCreated",
                table: "TaskWithClosedAnsw");
        }
    }
}
