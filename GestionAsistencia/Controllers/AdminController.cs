using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GestionAsistencia.Controllers
{
    [Authorize(Roles = "Superusuario")]
    public class AdminController : Controller
    {
        public IActionResult RegistrarEstudiante()
        {
            return View();
        }

        public IActionResult RegistrarProfesor()
        {
            return View();
        }

        public IActionResult MatricularEstudiante()
        {
            return View();
        }

        public IActionResult AsignarProfesor()
        {
            return View();
        }
    }

}
