using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class MedicineController : Controller
    {
        // GET: MedicineController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MedicineController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MedicineController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicineController/Create
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

        // GET: MedicineController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MedicineController/Edit/5
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

        // GET: MedicineController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MedicineController/Delete/5
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
