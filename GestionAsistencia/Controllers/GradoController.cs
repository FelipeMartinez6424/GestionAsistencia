using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionAsistencia.Controllers
{
    [Authorize(Roles = "Superusuario")]
    public class GradoController : Controller
    {
        private readonly AppDbContext _context;

        public GradoController(AppDbContext context)
        {
            _context = context;
        }

        // Listar grados
        public IActionResult Index()
        {
            var grados = _context.Grados.ToList();
            return View(grados);
        }

        // Crear grado (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Crear grado (POST)
        [HttpPost]
        public IActionResult Create(Grado grado)
        {
            if (ModelState.IsValid)
            {
                _context.Grados.Add(grado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grado);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var grado = _context.Grados.Find(id);
            if (grado == null)
            {
                return NotFound();
            }
            return View(grado);
        }

        [HttpPost]
        public IActionResult Edit(Grado grado)
        {
            if (ModelState.IsValid)
            {
                _context.Grados.Update(grado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grado);
        }


        // Eliminar grado (GET)
        public IActionResult Delete(int id)
        {
            var grado = _context.Grados.Find(id);
            if (grado == null) return NotFound();
            return View(grado);
        }

        // Eliminar grado (POST)
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var grado = _context.Grados.Find(id);
            if (grado != null)
            {
                _context.Grados.Remove(grado);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }

}
