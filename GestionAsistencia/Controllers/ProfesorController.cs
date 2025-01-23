using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GestionAsistencia.Controllers
{
    [Authorize(Roles = "Profesor")]
    public class ProfesorController : Controller
    {
        private readonly AppDbContext _context;

        public ProfesorController(AppDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar los cursos del día actual
        public IActionResult CursosDelDia()
        {
            var profesorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(profesorId))
            {
                Console.WriteLine("No se pudo obtener el ID del profesor.");
                return Content("No se pudo obtener el ID del profesor.");
            }

            int profesorIdInt = int.Parse(profesorId);

            var diaSemana = DateTime.Now.DayOfWeek switch
            {
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                DayOfWeek.Sunday => "Domingo",
                _ => ""
            };

            Console.WriteLine($"ProfesorId: {profesorIdInt}, DiaSemana: {diaSemana}");

            var horarios = _context.Horario
                .Where(h => h.ProfesorId == profesorIdInt && h.DiaSemana == diaSemana)
                .Include(h => h.Grado)
                .Include(h => h.Materia)
                .ToList();

            Console.WriteLine($"Cantidad de horarios encontrados: {horarios.Count}");

            if (!horarios.Any())
            {
                ViewBag.Mensaje = "No tienes clases asignadas para el día de hoy.";
            }

            return View(horarios);
        }




        // Acción para mostrar el listado de estudiantes de un curso específico
        public IActionResult ListaEstudiantes(int horarioId)
        {
            var estudiantes = _context.Estudiantes
                .Where(e => e.GradoId == _context.Horario.First(h => h.Id == horarioId).GradoId)
                .ToList();

            ViewBag.HorarioId = horarioId; // Enviar el horario seleccionado
            return View(estudiantes);
        }

        [HttpPost]
        public IActionResult GuardarAsistencia(int horarioId, IFormCollection form)
        {
            var estudiantesEnHorario = _context.Estudiantes
                .Where(e => e.GradoId == _context.Horario.First(h => h.Id == horarioId).GradoId)
                .ToList();

            var fechaHoy = DateTime.Now.Date;

            foreach (var estudiante in estudiantesEnHorario)
            {
                var estado = form[$"estado_{estudiante.Id}"]; // Obtén el estado seleccionado

                if (estado == "Inasistencia")
                {
                    // Registrar la inasistencia
                    var horario = _context.Horario
                        .Include(h => h.Grado)
                        .Include(h => h.Materia)
                        .FirstOrDefault(h => h.Id == horarioId);

                    _context.Inasistencias.Add(new Inasistencia
                    {
                        EstudianteId = estudiante.Id,
                        NombreEstudiante = estudiante.Nombre, // Nueva propiedad
                        Grado = horario.Grado.Nombre,
                        Materia = horario.Materia.Nombre,
                        Fecha = fechaHoy
                    });
                }
            }

            _context.SaveChanges();
            return RedirectToAction("CursosDelDia");
        }

        [HttpGet]
        public IActionResult Horario()
        {
            // Obtén el ID del profesor autenticado
            var profesorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Carga los horarios agrupados por día de la semana
            var horarios = _context.Horario
                .Where(h => h.ProfesorId == profesorId)
                .Include(h => h.Materia)
                .Include(h => h.Grado)
                .GroupBy(h => h.DiaSemana)
                .ToList();

            return View(horarios); // Asegúrate de que la vista exista
        }

    }
}
