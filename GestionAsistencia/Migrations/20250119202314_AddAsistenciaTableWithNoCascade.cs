using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddAsistenciaTableWithNoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HorarioId",
                table: "Asistencias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_HorarioId",
                table: "Asistencias",
                column: "HorarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencias_Horario_HorarioId",
                table: "Asistencias",
                column: "HorarioId",
                principalTable: "Horario",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistencias_Horario_HorarioId",
                table: "Asistencias");

            migrationBuilder.DropIndex(
                name: "IX_Asistencias_HorarioId",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "HorarioId",
                table: "Asistencias");
        }
    }
}
