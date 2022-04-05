using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web.Models;

namespace web.Controllers
{
    public class MedicineController : Controller
    {
        private readonly string _apiURL;

        public MedicineController(IConfiguration configuration)
        {
            _apiURL = configuration.GetValue<String>("DataServiceURL");
            Console.WriteLine(_apiURL);
        }

        // GET: MedicineController
        public async Task<ActionResult> IndexAsync()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(_apiURL + "/Medicine");

                var response = client.GetAsync(uri).Result;

                string result = await response.Content.ReadAsStringAsync();

                List<MedicineModel>? models = JsonConvert.DeserializeObject<List<MedicineModel>>(result);
                return View(models);
            }
        }

        // GET: MedicineController/Create
        public async Task<ActionResult> CreateAsync()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(_apiURL + "/DoseUnit");
                var response = client.GetAsync(uri).Result;
                string result = await response.Content.ReadAsStringAsync();

                List<String>? doseUnits = JsonConvert.DeserializeObject<List<String>>(result);

                uri = new Uri(_apiURL + "/MedicineColor");
                response = client.GetAsync(uri).Result;
                result = await response.Content.ReadAsStringAsync();

                List<String>? medicineColors = JsonConvert.DeserializeObject<List<String>>(result);

                uri = new Uri(_apiURL + "/MedicineType");
                response = client.GetAsync(uri).Result;
                result = await response.Content.ReadAsStringAsync();

                List<String>? medicineTypes = JsonConvert.DeserializeObject<List<String>>(result);

                uri = new Uri(_apiURL + "/MedicineShape");
                response = client.GetAsync(uri).Result;
                result = await response.Content.ReadAsStringAsync();

                List<String>? medicineShapes = JsonConvert.DeserializeObject<List<String>>(result);

                ViewBag.DoseUnits = doseUnits;
                ViewBag.MedicineColors = medicineColors;
                ViewBag.MedicineTypes = medicineTypes;
                ViewBag.MedicineShapes = medicineShapes;

                return View();
            }
        }

        // POST: MedicineController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(MedicineModel model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(_apiURL + "/Medicine");
                    var result = await client.PostAsJsonAsync(uri, model);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Medicijn aangemaakt!";
                    }
                    else
                    {
                        TempData["error"] = "Er is iets fout gegaan bij het aanmaken van het medicijn!";
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["error"] = "Er is iets fout gegaan bij het aanmaken van het medicijn!";
                return RedirectToAction("Index");
            }
        }

        // GET: MedicineController/CreateMedicineDoseUnit
        public ActionResult CreateMedicineDoseUnitAsync()
        {
            return View();
        }

        // POST: MedicineController/CreateMedicineDoseUnit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMedicineDoseUnitAsync(string doseUnit)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(_apiURL + "/DoseUnit");
                    var result = await client.PostAsJsonAsync(uri, doseUnit);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Dosering eenheid aangemaakt!";
                    }
                    else
                    {
                        TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de dosering eenheid!";
                    }
                }

            }
            catch
            {
                TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de dosering eenheid!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: MedicineController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id != 0)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var uri = new Uri(_apiURL + "/Medicine/" + id);
                        var result = await client.DeleteAsync(uri);

                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            TempData["success"] = "Medicijn is succesvol verwijderd!";

                        }
                        else
                        {
                            TempData["error"] = "Medicijn kon niet worden verwijderd!";
                        }
                    }
                }
                catch
                {
                    TempData["error"] = "Medicijn kon niet worden verwijderd!";
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}
