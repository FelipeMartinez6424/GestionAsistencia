using System.ComponentModel.DataAnnotations;

namespace GestionAsistencia.Models
{
    public class Asistencia
    {
        public int Id { get; set; }

        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }

        public int HorarioId { get; set; }
        public Horario Horario { get; set; }

        public DateTime Fecha { get; set; } // Fecha de la asistencia

        [Required]
        public string Estado { get; set; } // Asistió, Inasistencia, Retardo
    }

}
