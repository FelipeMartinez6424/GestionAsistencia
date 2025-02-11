using System.ComponentModel.DataAnnotations;

namespace GestionAsistencia.Models
{
    public class Asistencia
    {
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }

        [Required]
        public int HorarioId { get; set; }
        public Horario Horario { get; set; }

        [Required]
        public DateTime Fecha { get; set; } // Fecha de la asistencia

        [Required]
        public EstadoAsistencia Estado { get; set; } // Asistió, Inasistencia, Retardo
    }

    // Definir el estado de asistencia como un Enum
    public enum EstadoAsistencia
    {
        Asistio,
        Inasistencia,
        Retardo
    }
}
