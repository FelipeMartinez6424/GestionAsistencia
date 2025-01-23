using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAsistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddAcudienteToEstudiante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreAcudiente",
                table: "Estudiantes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreAcudiente",
                table: "Estudiantes");
        }
    }
}
