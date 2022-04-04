using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web.Models;

namespace web.Controllers
{
    public class MedicineController : Controller
    {
        private readonly String _apiURL;

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

        // GET: MedicineController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MedicineController/Create
        public async Task<ActionResult> CreateAsync()
        {
            using(var client = new HttpClient())
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
        public ActionResult Create(MedicineModel model)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
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
                return RedirectToAction(nameof(IndexAsync));
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
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}
