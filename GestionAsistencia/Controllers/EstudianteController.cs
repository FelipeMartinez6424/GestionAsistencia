using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionAsistencia.Controllers
{
    [Authorize(Roles = "Superusuario")]
    public class EstudianteController : Controller
    {
        private readonly AppDbContext _context;

        public EstudianteController(AppDbContext context)
        {
            _context = context;
        }

        // Listar estudiantes
        public IActionResult Index()
        {
            var estudiantes = _context.Estudiantes.Include(e => e.Grado).ToList();
            return View(estudiantes);
        }

        // Mostrar formulario de creación
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Grados = _context.Grados.ToList();
            return View();
        }

        // Procesar formulario de creación
        [HttpPost]
        public IActionResult Create(Estudiante estudiante)
        {
            // Ignorar la validación de la propiedad de navegación Grado
            ModelState.Remove(nameof(estudiante.Grado));

            if (ModelState.IsValid)
            {
                _context.Estudiantes.Add(estudiante);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Grados = _context.Grados.ToList();
            return View(estudiante);
        }

        // Mostrar formulario de edición
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var estudiante = _context.Estudiantes.Find(id);
            if (estudiante == null) return NotFound();

            //ViewBag.Grados = _context.Grados.ToList();
            ViewBag.Grados = new SelectList(_context.Grados, "Id", "Nombre", estudiante.GradoId);
            return View(estudiante);
        }

        // Procesar formulario de edición
        [HttpPost]
        public IActionResult Edit(Estudiante estudiante)
        {
            // Ignorar la validación de la propiedad de navegación Grado
            ModelState.Remove(nameof(estudiante.Grado));

            if (ModelState.IsValid)
            {
                _context.Estudiantes.Update(estudiante);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Grados = _context.Grados.ToList();
            return View(estudiante);
        }

        // Confirmar eliminación
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var estudiante = _context.Estudiantes.Include(e => e.Grado).FirstOrDefault(e => e.Id == id);
            if (estudiante == null) return NotFound();
            return View(estudiante);
        }

        // Procesar eliminación
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var estudiante = _context.Estudiantes.Find(id);
            if (estudiante != null)
            {
                _context.Estudiantes.Remove(estudiante);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }

}
