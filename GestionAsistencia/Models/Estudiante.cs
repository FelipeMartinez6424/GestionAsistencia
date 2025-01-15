namespace GestionAsistencia.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
        public string ContactoPadres { get; set; }
    }

}
