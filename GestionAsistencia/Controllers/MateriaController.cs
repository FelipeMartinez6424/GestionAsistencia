using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionAsistencia.Controllers
{
    [Authorize(Roles = "Superusuario")]
    public class MateriaController : Controller
    {
        private readonly AppDbContext _context;

        public MateriaController(AppDbContext context)
        {
            _context = context;
        }

        // Listar materias
        public IActionResult Index()
        {
            var materias = _context.Materias.ToList();
            return View(materias);
        }

        // Mostrar formulario de creación
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Procesar formulario de creación
        [HttpPost]
        public IActionResult Create(Materia materia)
        {
            if (ModelState.IsValid)
            {
                _context.Materias.Add(materia);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(materia);
        }

        // Mostrar formulario de edición
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var materia = _context.Materias.Find(id);
            if (materia == null) return NotFound();
            return View(materia);
        }

        // Procesar formulario de edición
        [HttpPost]
        public IActionResult Edit(Materia materia)
        {
            if (ModelState.IsValid)
            {
                _context.Materias.Update(materia);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(materia);
        }

        // Confirmar eliminación
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var materia = _context.Materias.Find(id);
            if (materia == null) return NotFound();
            return View(materia);
        }

        // Procesar eliminación
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var materia = _context.Materias.Find(id);
            if (materia != null)
            {
                _context.Materias.Remove(materia);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Mostrar formulario para asociar grados a una materia
        [HttpGet]
        public IActionResult AsociarGrado (int id)
        {
            var materia = _context.Materias
                .Include(m => m.Grados)
                .FirstOrDefault(m => m.Id == id);

            if (materia == null)
            {
                return NotFound();
            }

            // Obtener todos los grados y marcar los que ya están asociados
            ViewBag.Grados = _context.Grados.ToList();
            ViewBag.GradosAsociados = materia.Grados.Select(g => g.Id).ToList();

            return View(materia);
        }

        // Procesar la asociación de grados a una materia
        [HttpPost]
        public IActionResult AsociarGrado(int id, List<int> gradoIds)
        {
            var materia = _context.Materias
                .Include(m => m.Grados)
                .FirstOrDefault(m => m.Id == id);

            if (materia == null)
            {
                return NotFound();
            }

            // Actualizar las asociaciones
            materia.Grados.Clear();
            foreach (var gradoId in gradoIds)
            {
                var grado = _context.Grados.Find(gradoId);
                if (grado != null)
                {
                    materia.Grados.Add(grado);
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }

}
