using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Profesores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    idProfesor = table.Column<int>(type: "int", nullable: false),
                    idEstudiante = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notas_Estudiantes_idEstudiante",
                        column: x => x.idEstudiante,
                        principalTable: "Estudiantes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notas_Profesores_idProfesor",
                        column: x => x.idProfesor,
                        principalTable: "Profesores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notas_idEstudiante",
                table: "Notas",
                column: "idEstudiante");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_idProfesor",
                table: "Notas",
                column: "idProfesor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notas");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Profesores");
        }
    }
}
