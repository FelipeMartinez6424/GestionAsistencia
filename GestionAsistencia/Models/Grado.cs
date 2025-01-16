using System.ComponentModel.DataAnnotations;

namespace GestionAsistencia.Models
{
    public class Grado
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del grado es obligatorio.")]
        public string Nombre { get; set; }
        // Inicializar como listas vacías para evitar problemas de validación
        public ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();
        public ICollection<Materia> Materias { get; set; } = new List<Materia>();
    }

}
