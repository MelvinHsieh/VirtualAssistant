using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web.Models;

namespace web.Controllers
{
    public class CareWorkerController : Controller
    {
        private readonly string _apiURL;

        public CareWorkerController(IConfiguration configuration)
        {
            _apiURL = configuration.GetValue<String>("DataServiceURL");
        }

        // GET: CareWorkerController
        public async Task<ActionResult> IndexAsync()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(_apiURL + "/CareWorker");
                try
                {
                    var response = client.GetAsync(uri).Result;

                    string result = await response.Content.ReadAsStringAsync();

                    List<CareWorkerModel>? models = JsonConvert.DeserializeObject<List<CareWorkerModel>>(result);
                    return View(models);
                }
                catch
                {
                    TempData["error"] = "De zorgmedewerkers konden niet opgehaald worden. Controleer de dataservice!";
                    return View();
                }
            }
        }

        // GET: CareWorkerController/Create
        public async Task<ActionResult> CreateAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var uri = new Uri(_apiURL + "/CareWorker/function");
                    var response = client.GetAsync(uri).Result;
                    string result = await response.Content.ReadAsStringAsync();

                    List<String>? functions = JsonConvert.DeserializeObject<List<String>>(result);

                    ViewBag.CareWorkerFunctions = functions;

                    return View();
                }
                catch
                {
                    TempData["error"] = "De zorgmedewerkergegevens konden niet opgehaald worden. Controleer de dataservice!";
                    return RedirectToAction("Index");
                }
            }
        }

        // POST: CareWorkerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CareWorkerModel model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(_apiURL + "/CareWorker");
                    var result = await client.PostAsJsonAsync(uri, model);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Zorgmedewerker aangemaakt!";
                    }
                    else
                    {
                        TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de zorgmedewerker!";
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de zorgmedewerker!";
                return RedirectToAction("Index");
            }
        }

        // GET: CareWorkerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CareWorkerController/Edit/5
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

        // GET: CareWorkerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CareWorkerController/Delete/5
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
