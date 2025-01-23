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
            var ranking = _context.Inasistencias
                .GroupBy(i => new { i.EstudianteId, i.NombreEstudiante, i.Grado })
                .Select(g => new
                {
                    EstudianteId = g.Key.EstudianteId,
                    Nombre = g.Key.NombreEstudiante, // Usar NombreEstudiante en lugar de Estudiante
                    Grado = g.Key.Grado,
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
            // Obtén los detalles de inasistencias
            var detalles = _context.Inasistencias
                .Where(i => i.EstudianteId == estudianteId)
                .OrderByDescending(i => i.Fecha)
                .ToList();

            // Obtén los datos del estudiante
            var estudiante = _context.Estudiantes.FirstOrDefault(e => e.Id == estudianteId);

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

            // Filtrar inasistencias por curso y rango de fechas
            var inasistencias = _context.Inasistencias
                .Where(i => i.Grado == _context.Grados.FirstOrDefault(g => g.Id == gradoId).Nombre &&
                            i.Fecha >= fechaInicio &&
                            i.Fecha <= fechaFin)
                .OrderBy(i => i.Fecha)
                .ToList();

            ViewBag.CursoSeleccionado = _context.Grados.FirstOrDefault(g => g.Id == gradoId)?.Nombre;
            ViewBag.FechaInicio = fechaInicio.ToString("dd/MM/yyyy");
            ViewBag.FechaFin = fechaFin.ToString("dd/MM/yyyy");

            return View("ResultadosInasistencias", inasistencias);
        }


    }
}
