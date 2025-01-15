namespace GestionAsistencia.Models
{
    public class Asistencia
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } // Asistió, No asistió, Retardo
    }

}
