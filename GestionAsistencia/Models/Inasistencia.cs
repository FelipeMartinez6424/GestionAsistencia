namespace GestionAsistencia.Models
{
    public class Inasistencia
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public string NombreEstudiante { get; set; } // Nueva propiedad
        public string Grado { get; set; }
        public string Materia { get; set; }
        public DateTime Fecha { get; set; }
    }




}
