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

        // GET: CareWorkerController/CreateMedicineDoseUnit
        public ActionResult CreateCareWorkerFunction()
        {
            return View();
        }

        // POST: CareWorkerController/CreateCareWorkerFunction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCareWorkerFunctionAsync(string careWorkerFunction)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(_apiURL + "/CareWorker/function");
                    var result = await client.PostAsJsonAsync(uri, careWorkerFunction);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Zorgmedewerker functie aangemaakt!";
                    }
                    else
                    {
                        TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de zorgmedewerker functie!";
                    }
                }

            }
            catch
            {
                TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de zorgmedewerker functie!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: CareWorkerController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id != 0)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var uri = new Uri(_apiURL + "/CareWorker/" + id);
                        var result = await client.DeleteAsync(uri);

                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            TempData["success"] = "Zorgmedewerker succesvol verwijderd!";

                        }
                        else
                        {
                            TempData["error"] = "Zorgmedewerker kon niet worden verwijderd!";
                        }
                    }
                }
                catch
                {
                    TempData["error"] = "Zorgmedewerker kon niet worden verwijderd!";
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
