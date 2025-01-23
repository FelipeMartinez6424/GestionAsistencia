using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsistencia.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInasistenciaModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Inasistencias_EstudianteId",
                table: "Inasistencias",
                column: "EstudianteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inasistencias_Estudiantes_EstudianteId",
                table: "Inasistencias",
                column: "EstudianteId",
                principalTable: "Estudiantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inasistencias_Estudiantes_EstudianteId",
                table: "Inasistencias");

            migrationBuilder.DropIndex(
                name: "IX_Inasistencias_EstudianteId",
                table: "Inasistencias");
        }
    }
}
