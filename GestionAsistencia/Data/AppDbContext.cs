using GestionAsistencia.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionAsistencia.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Grado> Grados { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<ProfesorMateriaGrado> ProfesorMateriaGrados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración muchos a muchos entre Grado y Materia
            modelBuilder.Entity<Grado>()
                .HasMany(g => g.Materias)
                .WithMany(m => m.Grados);

            // Configuración de la tabla intermedia
            modelBuilder.Entity<ProfesorMateriaGrado>()
                .HasOne(pmg => pmg.Usuario)
                .WithMany(u => u.ProfesorMateriaGrados)
                .HasForeignKey(pmg => pmg.UsuarioId);

            modelBuilder.Entity<ProfesorMateriaGrado>()
                .HasOne(pmg => pmg.Materia)
                .WithMany(m => m.ProfesorMateriaGrados)
                .HasForeignKey(pmg => pmg.MateriaId);

            modelBuilder.Entity<ProfesorMateriaGrado>()
                .HasOne(pmg => pmg.Grado)
                .WithMany()
                .HasForeignKey(pmg => pmg.GradoId);
        }
    }

}
