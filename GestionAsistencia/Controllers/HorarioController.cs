using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionAsistencia.Controllers
{
    [Authorize(Roles = "Superusuario")]
    public class HorarioController : Controller
    {
        private readonly AppDbContext _context;

        public HorarioController(AppDbContext context)
        {
            _context = context;
        }

        // Mostrar formulario para asociar
        [HttpGet]
        public IActionResult Asociar()
        {
            ViewBag.Profesores = _context.Usuarios.Where(u => u.Rol == "Profesor").ToList();
            ViewBag.Materias = _context.Materias.ToList();
            ViewBag.Grados = _context.Grados.ToList();
            return View();
        }

        // Procesar asociación
        [HttpPost]
        public IActionResult Asociar(int usuarioId, int materiaId, int gradoId, string diaSemana)
        {
            // Validar los parámetros
            if (string.IsNullOrEmpty(diaSemana))
            {
                TempData["Error"] = "Debe seleccionar un día de la semana.";
                return RedirectToAction("Asociar");
            }

            // Verificar si ya existe una asociación similar
            var existe = _context.Horario.Any(h =>
                h.ProfesorId == usuarioId &&
                h.MateriaId == materiaId &&
                h.GradoId == gradoId &&
                h.DiaSemana == diaSemana);

            if (existe)
            {
                TempData["Error"] = "Esta asociación ya existe.";
                return RedirectToAction("Asociar");
            }

            // Crear la asociación
            var horario = new Horario
            {
                ProfesorId = usuarioId,
                MateriaId = materiaId,
                GradoId = gradoId,
                DiaSemana = diaSemana
            };

            _context.Horario.Add(horario);
            _context.SaveChanges();

            TempData["Success"] = "Asociación guardada correctamente.";
            return RedirectToAction("Asociar");
        }


        // Listar asociaciones
        public IActionResult Index()
        {
            var asociaciones = _context.Horario
                .Include(pmg => pmg.Profesor) // Incluye el objeto Profesor
                .Include(pmg => pmg.Materia) // Incluye el objeto Materia
                .Include(pmg => pmg.Grado)   // Incluye el objeto Grado
                .ToList();

            return View(asociaciones);
        }
    }

}
