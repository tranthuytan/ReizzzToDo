using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReizzzToDo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "time_units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_time_units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredTimeUnit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "to_dos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    TimeToComplete = table.Column<float>(type: "real", nullable: false),
                    TimeUnitId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_to_dos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_to_dos_time_units_TimeUnitId",
                        column: x => x.TimeUnitId,
                        principalTable: "time_units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_todo_user",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "time_units",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Seconds" },
                    { 2, "Minutes" },
                    { 3, "Hours" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_to_dos_TimeUnitId",
                table: "to_dos",
                column: "TimeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_to_dos_UserId",
                table: "to_dos",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "to_dos");

            migrationBuilder.DropTable(
                name: "time_units");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
