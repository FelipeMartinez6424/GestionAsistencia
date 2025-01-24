using GestionAsistencia.Data;
using GestionAsistencia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionAsistencia.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // Listar usuarios
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        // Crear usuario (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Crear usuario (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario)
        {

            if (ModelState.IsValid)
            {
                //usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password); // Hashear la contraseña
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }
    }
}
