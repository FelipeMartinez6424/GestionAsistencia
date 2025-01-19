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
        public DbSet<Horario> Horario { get; set; }
        public DbSet<Inasistencia> Inasistencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Horario>().ToTable("Horario");
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
            modelBuilder.Entity<Horario>()
           .HasOne(h => h.Profesor)
           .WithMany()
           .HasForeignKey(h => h.ProfesorId);

            modelBuilder.Entity<Horario>()
                .HasOne(h => h.Materia)
                .WithMany()
                .HasForeignKey(h => h.MateriaId);

            modelBuilder.Entity<Horario>()
                .HasOne(h => h.Grado)
                .WithMany()
                .HasForeignKey(h => h.GradoId);
            modelBuilder.Entity<Asistencia>()
                .HasOne(a => a.Horario)
                .WithMany()
                .HasForeignKey(a => a.HorarioId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
