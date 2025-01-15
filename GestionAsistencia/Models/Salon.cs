namespace GestionAsistencia.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Jornada { get; set; }
        public ICollection<Estudiante> Estudiantes { get; set; }
    }

}
