using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsistencia.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudiantes_Salones_SalonId",
                table: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Salones");

            migrationBuilder.RenameColumn(
                name: "SalonId",
                table: "Estudiantes",
                newName: "GradoId");

            migrationBuilder.RenameIndex(
                name: "IX_Estudiantes_SalonId",
                table: "Estudiantes",
                newName: "IX_Estudiantes_GradoId");

            migrationBuilder.CreateTable(
                name: "Grados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradoMateria",
                columns: table => new
                {
                    GradosId = table.Column<int>(type: "int", nullable: false),
                    MateriasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradoMateria", x => new { x.GradosId, x.MateriasId });
                    table.ForeignKey(
                        name: "FK_GradoMateria_Grados_GradosId",
                        column: x => x.GradosId,
                        principalTable: "Grados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradoMateria_Materias_MateriasId",
                        column: x => x.MateriasId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfesorMateriaGrados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    GradoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfesorMateriaGrados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfesorMateriaGrados_Grados_GradoId",
                        column: x => x.GradoId,
                        principalTable: "Grados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfesorMateriaGrados_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfesorMateriaGrados_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradoMateria_MateriasId",
                table: "GradoMateria",
                column: "MateriasId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfesorMateriaGrados_GradoId",
                table: "ProfesorMateriaGrados",
                column: "GradoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfesorMateriaGrados_MateriaId",
                table: "ProfesorMateriaGrados",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfesorMateriaGrados_UsuarioId",
                table: "ProfesorMateriaGrados",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudiantes_Grados_GradoId",
                table: "Estudiantes",
                column: "GradoId",
                principalTable: "Grados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudiantes_Grados_GradoId",
                table: "Estudiantes");

            migrationBuilder.DropTable(
                name: "GradoMateria");

            migrationBuilder.DropTable(
                name: "ProfesorMateriaGrados");

            migrationBuilder.DropTable(
                name: "Grados");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.RenameColumn(
                name: "GradoId",
                table: "Estudiantes",
                newName: "SalonId");

            migrationBuilder.RenameIndex(
                name: "IX_Estudiantes_GradoId",
                table: "Estudiantes",
                newName: "IX_Estudiantes_SalonId");

            migrationBuilder.CreateTable(
                name: "Salones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jornada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salones", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Estudiantes_Salones_SalonId",
                table: "Estudiantes",
                column: "SalonId",
                principalTable: "Salones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
