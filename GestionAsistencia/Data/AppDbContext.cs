using GestionAsistencia.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionAsistencia.Data
{
   public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Salon> Salones { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
    }
}
