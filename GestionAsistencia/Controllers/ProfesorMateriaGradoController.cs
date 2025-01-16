using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionAsistencia.Controllers
{
    [Authorize(Roles = "Superusuario")]
    public class ProfesorMateriaGradoController : Controller
    {
        private readonly AppDbContext _context;

        public ProfesorMateriaGradoController(AppDbContext context)
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
        public IActionResult Asociar(int usuarioId, int materiaId, int gradoId)
        {
            if (_context.ProfesorMateriaGrados.Any(pmg => pmg.UsuarioId == usuarioId && pmg.MateriaId == materiaId && pmg.GradoId == gradoId))
            {
                TempData["Error"] = "Esta asociación ya existe.";
                return RedirectToAction("Asociar");
            }

            var asociacion = new ProfesorMateriaGrado
            {
                UsuarioId = usuarioId,
                MateriaId = materiaId,
                GradoId = gradoId
            };

            _context.ProfesorMateriaGrados.Add(asociacion);
            _context.SaveChanges();

            TempData["Success"] = "La asociación se creó correctamente.";
            return RedirectToAction("Asociar");
        }

        // Listar asociaciones
        public IActionResult Index()
        {
            var asociaciones = _context.ProfesorMateriaGrados
                .Include(pmg => pmg.Usuario)
                .Include(pmg => pmg.Materia)
                .Include(pmg => pmg.Grado)
                .ToList();
            return View(asociaciones);
        }
    }

}
