namespace GestionAsistencia.Models
{
    public class ProfesorMateriaGrado
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int MateriaId { get; set; }
        public Materia Materia { get; set; }

        public int GradoId { get; set; }
        public Grado Grado { get; set; }
    }

}
