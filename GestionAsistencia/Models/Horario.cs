using System.ComponentModel.DataAnnotations;

namespace GestionAsistencia.Models
{
    public class Horario
    {
        public int Id { get; set; }

        public int ProfesorId { get; set; }
        public Usuario Profesor { get; set; } // Usuario con rol Profesor

        public int MateriaId { get; set; }
        public Materia Materia { get; set; }

        public int GradoId { get; set; }
        public Grado Grado { get; set; }

        [Required]
        public string DiaSemana { get; set; } // Lunes, Martes, Miércoles, etc.
    }

}
