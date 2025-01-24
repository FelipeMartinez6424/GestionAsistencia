using System.ComponentModel.DataAnnotations;

namespace GestionAsistencia.Models
{
    public class Estudiante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El grado es obligatorio.")]
        public int GradoId { get; set; }

        public Grado Grado { get; set; }

        [Required(ErrorMessage = "El contacto de los padres es obligatorio.")]
        [Phone(ErrorMessage = "El formato del número de contacto no es válido.")]
        public string ContactoPadres { get; set; }
        public string NombreAcudiente { get; set; } // Nombre del acudiente
        //public ICollection<Inasistencia> Inasistencias { get; set; }
    }


}
