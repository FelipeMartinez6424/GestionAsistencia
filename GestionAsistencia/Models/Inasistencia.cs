namespace GestionAsistencia.Models
{
    public class Inasistencia
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public string NombreEstudiante { get; set; } // Nueva propiedad
                                                     // Clave foránea a Grado
        public int GradoId { get; set; }
        public Grado Grado { get; set; }

        // Clave foránea a Materia
        public int MateriaId { get; set; }
        public Materia Materia { get; set; }
        public DateTime Fecha { get; set; }
    }

}
