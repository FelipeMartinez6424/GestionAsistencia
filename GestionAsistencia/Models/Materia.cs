namespace GestionAsistencia.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // Ejemplo: "Matemáticas"
        public ICollection<Grado> Grados { get; set; }= new List<Grado>();// Relación muchos a muchos
    }

}
