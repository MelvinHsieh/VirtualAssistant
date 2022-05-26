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
        }

        // GET: MedicineController
        public async Task<ActionResult> IndexAsync()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(_apiURL + "/Medicine");
                try
                {
                    var response = client.GetAsync(uri).Result;

                    string result = await response.Content.ReadAsStringAsync();

                    List<MedicineModel>? models = JsonConvert.DeserializeObject<List<MedicineModel>>(result);
                    return View(models);
                }
                catch
                {
                    TempData["error"] = "De medicijnen konden niet opgehaald worden. Controleer de dataservice!";

                    return View();
                }

            }
        }

        // GET: MedicineController/Create
        public async Task<ActionResult> CreateAsync()
        {
            using (var client = new HttpClient())
            {
                try
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
                catch
                {
                    TempData["error"] = "De medicijnengegevens konden niet opgehaald worden. Controleer de dataservice!";
                    return RedirectToAction("Index");
                }
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

        // GET: MedicineController/CreateMedicineDoseUnit
        public ActionResult CreateMedicineDoseUnit()
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

        // GET: MedicineController/CreateMedicineColor
        public ActionResult CreateMedicineColor()
        {
            return View();
        }

        // POST: MedicineController/CreateMedicineColor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMedicineColorAsync(string color)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(_apiURL + "/MedicineColor");
                    var result = await client.PostAsJsonAsync(uri, color);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Medicijn kleur aangemaakt!";
                    }
                    else
                    {
                        TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de medicijn kleur!";
                    }
                }

            }
            catch
            {
                TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de medicijn kleur!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: MedicineController/CreateMedicineShape
        public ActionResult CreateMedicineShape()
        {
            return View();
        }

        // POST: MedicineController/CreateMedicineShape
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMedicineShapeAsync(string shape)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(_apiURL + "/MedicineShape");
                    var result = await client.PostAsJsonAsync(uri, shape);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Medicijn vorm aangemaakt!";
                    }
                    else
                    {
                        TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de medicijn vorm!";
                    }
                }

            }
            catch
            {
                TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de medicijn vorm!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: MedicineController/CreateMedicineType
        public ActionResult CreateMedicineType()
        {
            return View();
        }

        // POST: MedicineController/CreateMedicineType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMedicineTypeAsync(string type)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(_apiURL + "/MedicineType");
                    var result = await client.PostAsJsonAsync(uri, type);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Medicijn type aangemaakt!";
                    }
                    else
                    {
                        TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de medicijn type!";
                    }
                }

            }
            catch
            {
                TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de medicijn type!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
