using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web.Models;

namespace web.Controllers
{
    public class PatientController : Controller
    {

        private readonly string _apiURL;

        public PatientController(IConfiguration configuration)
        {
            _apiURL = configuration.GetValue<String>("DataServiceURL");
            Console.WriteLine(_apiURL);
        }

        // GET: PatientController
        public async Task<ActionResult> IndexAsync()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(_apiURL + "/Medicine");

                var response = client.GetAsync(uri).Result;

                string result = await response.Content.ReadAsStringAsync();

                List<PatientModel>? models = JsonConvert.DeserializeObject<List<PatientModel>>(result);
                return View(models);
            }
        }

        // GET: PatientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PatientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientController/Create
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

        // GET: PatientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatientController/Edit/5
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

        // GET: PatientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientController/Delete/5
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
