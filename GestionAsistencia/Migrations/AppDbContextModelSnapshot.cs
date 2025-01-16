﻿// <auto-generated />
using System;
using GestionAsistencia.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GestionAsistencia.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GestionAsistencia.Models.Asistencia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EstudianteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EstudianteId");

                    b.ToTable("Asistencias");
                });

            modelBuilder.Entity("GestionAsistencia.Models.Estudiante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContactoPadres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GradoId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GradoId");

                    b.ToTable("Estudiantes");
                });

            modelBuilder.Entity("GestionAsistencia.Models.Grado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Grados");
                });

            modelBuilder.Entity("GestionAsistencia.Models.Materia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Materias");
                });

            modelBuilder.Entity("GestionAsistencia.Models.ProfesorMateriaGrado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GradoId")
                        .HasColumnType("int");

                    b.Property<int>("MateriaId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GradoId");

                    b.HasIndex("MateriaId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("ProfesorMateriaGrados");
                });

            modelBuilder.Entity("GestionAsistencia.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("GradoMateria", b =>
                {
                    b.Property<int>("GradosId")
                        .HasColumnType("int");

                    b.Property<int>("MateriasId")
                        .HasColumnType("int");

                    b.HasKey("GradosId", "MateriasId");

                    b.HasIndex("MateriasId");

                    b.ToTable("GradoMateria");
                });

            modelBuilder.Entity("GestionAsistencia.Models.Asistencia", b =>
                {
                    b.HasOne("GestionAsistencia.Models.Estudiante", "Estudiante")
                        .WithMany()
                        .HasForeignKey("EstudianteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estudiante");
                });

            modelBuilder.Entity("GestionAsistencia.Models.Estudiante", b =>
                {
                    b.HasOne("GestionAsistencia.Models.Grado", "Grado")
                        .WithMany("Estudiantes")
                        .HasForeignKey("GradoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grado");
                });

            modelBuilder.Entity("GestionAsistencia.Models.ProfesorMateriaGrado", b =>
                {
                    b.HasOne("GestionAsistencia.Models.Grado", "Grado")
                        .WithMany()
                        .HasForeignKey("GradoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionAsistencia.Models.Materia", "Materia")
                        .WithMany("ProfesorMateriaGrados")
                        .HasForeignKey("MateriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionAsistencia.Models.Usuario", "Usuario")
                        .WithMany("ProfesorMateriaGrados")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grado");

                    b.Navigation("Materia");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GradoMateria", b =>
                {
                    b.HasOne("GestionAsistencia.Models.Grado", null)
                        .WithMany()
                        .HasForeignKey("GradosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionAsistencia.Models.Materia", null)
                        .WithMany()
                        .HasForeignKey("MateriasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GestionAsistencia.Models.Grado", b =>
                {
                    b.Navigation("Estudiantes");
                });

            modelBuilder.Entity("GestionAsistencia.Models.Materia", b =>
                {
                    b.Navigation("ProfesorMateriaGrados");
                });

            modelBuilder.Entity("GestionAsistencia.Models.Usuario", b =>
                {
                    b.Navigation("ProfesorMateriaGrados");
                });
#pragma warning restore 612, 618
        }
    }
}
