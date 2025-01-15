using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionAsistencia.Controllers
{
    public class RectorController : Controller
    {
        // GET: RectorController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RectorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RectorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RectorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RectorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RectorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RectorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RectorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
