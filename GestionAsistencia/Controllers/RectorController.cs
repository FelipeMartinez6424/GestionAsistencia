using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GestionAsistencia.Controllers
{
    [Authorize(Roles = "Rector")]
    public class RectorController : Controller
    {
        private readonly AppDbContext _context;

        public RectorController(AppDbContext context)
        {
            _context = context;
        }


        // Ranking de estudiantes con más inasistencias
        public IActionResult Ranking(string filtroNombre = "")
        {
            var ranking = _context.Asistencias
                .Include(a => a.Estudiante)
                .Include(a => a.Horario)
                    .ThenInclude(h => h.Grado) // Incluir la relación con Grado
                .Where(a => a.Estado == EstadoAsistencia.Inasistencia) // Filtrar solo inasistencias
                .GroupBy(a => new { a.EstudianteId, a.Estudiante.Nombre, GradoNombre = a.Horario.Grado.Nombre }) // Alias para Grado.Nombre
                .Select(g => new
                {
                    EstudianteId = g.Key.EstudianteId,
                    Nombre = g.Key.Nombre,
                    Grado = g.Key.GradoNombre, // Acceder al alias
                    TotalInasistencias = g.Count()
                })
                .OrderByDescending(g => g.TotalInasistencias)
                .Take(50)
                .ToList();

            if (!string.IsNullOrEmpty(filtroNombre))
            {
                ranking = ranking.Where(r => r.Nombre.Contains(filtroNombre, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(ranking);
        }

        public IActionResult DetalleInasistencias(int estudianteId)
        {
            // Obtén los detalles de asistencias marcadas como inasistencia
            var detalles = _context.Asistencias
                .Include(a => a.Horario)
                    .ThenInclude(h => h.Materia)
                .Where(a => a.EstudianteId == estudianteId && a.Estado == EstadoAsistencia.Inasistencia)
                .OrderByDescending(a => a.Fecha)
                .ToList();

            // Obtén los datos del estudiante
            var estudiante = _context.Estudiantes
                .Include(e => e.Grado) // Incluir la relación con Grado
                .FirstOrDefault(e => e.Id == estudianteId);

            if (estudiante != null)
            {
                ViewBag.NombreEstudiante = estudiante.Nombre;
                ViewBag.NombreAcudiente = estudiante.NombreAcudiente;
                ViewBag.ContactoPadres = estudiante.ContactoPadres;
            }
            else
            {
                ViewBag.NombreEstudiante = "Estudiante no encontrado";
                ViewBag.NombreAcudiente = "N/A";
                ViewBag.ContactoPadres = "N/A";
            }

            return View(detalles);
        }

        public IActionResult BuscarInasistencias()
        {
            // Obtener los cursos disponibles
            var cursos = _context.Grados.ToList();
            ViewBag.Cursos = cursos;

            return View();
        }

        [HttpPost]
        public IActionResult BuscarInasistencias(int gradoId, DateTime fechaInicio, DateTime fechaFin)
        {
            // Validar datos
            if (gradoId == 0 || fechaInicio == default || fechaFin == default)
            {
                TempData["Error"] = "Todos los campos son obligatorios.";
                return RedirectToAction("BuscarInasistencias");
            }

            if (fechaInicio > fechaFin)
            {
                TempData["Error"] = "La fecha de inicio no puede ser mayor a la fecha de fin.";
                return RedirectToAction("BuscarInasistencias");
            }

            // Obtener el nombre del grado
            var grado = _context.Grados.FirstOrDefault(g => g.Id == gradoId);
            if (grado == null)
            {
                TempData["Error"] = "El grado seleccionado no existe.";
                return RedirectToAction("BuscarInasistencias");
            }

            // Filtrar asistencias marcadas como "Inasistencia" en el rango de fechas
            var inasistencias = _context.Asistencias
                .Include(a => a.Estudiante)
                .Include(a => a.Horario)
                    .ThenInclude(h => h.Grado)
                .Include(a => a.Horario)
                    .ThenInclude(h => h.Materia)
                .Where(a => a.Horario.GradoId == gradoId &&
                            a.Fecha >= fechaInicio &&
                            a.Fecha <= fechaFin &&
                            a.Estado == EstadoAsistencia.Inasistencia) // Filtrar solo inasistencias
                .OrderBy(a => a.Fecha)
                .ToList();

            ViewBag.CursoSeleccionado = grado.Nombre;
            ViewBag.FechaInicio = fechaInicio.ToString("dd/MM/yyyy");
            ViewBag.FechaFin = fechaFin.ToString("dd/MM/yyyy");

            return View("ResultadosInasistencias", inasistencias);
        }


    }
}
