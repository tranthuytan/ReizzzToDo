using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReizzzToDo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_ToDo_TimeUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_to_dos_time_units_TimeUnitId",
                table: "to_dos");

            migrationBuilder.DropColumn(
                name: "PreferredTimeUnit",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "PreferredTimeUnitId",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_todo_time_unit",
                table: "to_dos",
                column: "TimeUnitId",
                principalTable: "time_units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_todo_time_unit",
                table: "to_dos");

            migrationBuilder.DropColumn(
                name: "PreferredTimeUnitId",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "PreferredTimeUnit",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_to_dos_time_units_TimeUnitId",
                table: "to_dos",
                column: "TimeUnitId",
                principalTable: "time_units",
                principalColumn: "Id");
        }
    }
}
