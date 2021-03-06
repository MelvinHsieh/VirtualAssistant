using Microsoft.AspNetCore.Mvc;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using web.Models;
using web.Models.Common;
using web.Models.CreateModels;
using web.Utils;
using web.Models.ViewModels;
using System.Net.Http.Headers;

namespace web.Controllers
{
    [Authorize(Roles = Roles.Personnel)]
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _apiURL;
        private readonly string _authURL;

        public PatientController(IConfiguration configuration, ILogger<PatientController> logger)
        {
            _configuration = configuration;
            _apiURL = configuration.GetValue<String>("DataServiceURL");
            _logger = logger;
            _authURL = configuration.GetValue<String>("AuthURL");
        }

        // GET: PatientController
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                using (var client = new AuthHttpClient(User))
                {
                    var uri = new Uri(_apiURL + "/Patient");

                    var response = client.GetAsync(uri).Result;

                    string result = await response.Content.ReadAsStringAsync();

                    try
                    {
                        List<PatientModel>? models = JsonConvert.DeserializeObject<List<PatientModel>>(result);
                        if (models != null)
                            return View(models);
                        else
                        {
                            TempData["error"] = "Response: " + response.StatusCode;
                            return View();
                        }
                    }
                    catch (Exception e)
                    {
                        if (IsValidJson(result))
                            TempData["error"] = e.Message;
                        else
                            TempData["error"] = "Result was not valid JSON data, could be an SQL error";
                        return View();
                    }
                }
            }
            catch
            {
                TempData["error"] = "Geen connectie kon gemaakt worden met de Dataservice.";
                return View();
            }
        }

        // GET: PatientController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (id > 0)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AdminToken"]);
                        var uri = new Uri($"{_apiURL}/Patient/{id}");
                        var response = client.GetAsync(uri).Result;
                        string result = await response.Content.ReadAsStringAsync();
                        PatientModel? patient = JsonConvert.DeserializeObject<PatientModel>(result);

                        uri = new Uri($"{_apiURL}/PatientIntake/patient/{id}");
                        response = client.GetAsync(uri).Result;
                        result = await response.Content.ReadAsStringAsync();
                        List<IntakeModel>? intake = JsonConvert.DeserializeObject<List<IntakeModel>>(result);

                        return View("Details", new PatientDetailsViewModel(patient, intake));
                    }
                }
                catch
                {
                    TempData["error"] = "Ophalen van patiëntgegevens is mislukt!";
                }
            }
            return RedirectToAction(nameof(IndexAsync));
        }

        // GET: PatientController/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: PatientController/Show/5
        public async Task<ActionResult> CreateIntake(int id)
        {
            if (id > 0)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AdminToken"]);
                        var uri = new Uri(_apiURL + "/Patient/" + id);
                        var response = client.GetAsync(uri).Result;
                        string result = await response.Content.ReadAsStringAsync();
                        PatientModel? patient = JsonConvert.DeserializeObject<PatientModel>(result);

                        uri = new Uri(_apiURL + "/Medicine");
                        response = client.GetAsync(uri).Result;
                        result = await response.Content.ReadAsStringAsync();
                        List<MedicineModel>? models = JsonConvert.DeserializeObject<List<MedicineModel>>(result);
                        SetMedicineBag(models);

                        return View(Tuple.Create(patient, new IntakeModel()));
                    }
                }
                catch
                {
                    TempData["error"] = "Ophalen van patiënt is mislukt!";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // helper method for the SHOW method
        private void SetMedicineBag(List<MedicineModel>? models)
        {
            if (models != null)
            {
                var references = models.AsEnumerable().OrderBy(o => o.Id);

                List<SelectListItem> medicineItems = references.Select(r =>
                    new SelectListItem()
                    {
                        Value = r.Id.ToString(),
                        Text = r.Name
                    }).ToList();

                ViewBag.Medicine = new SelectList(medicineItems, "Value", "Text");
            }
            else
                ViewBag.Medicine = new SelectList(null);
        }

        // POST: PatientController/CreateIntake/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateIntakeAsync(IntakeModel model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AdminToken"]);
                    var uri = new Uri(_apiURL + "/PatientIntake");
                    var result = await client.PostAsJsonAsync(uri, model);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        TempData["success"] = "Inname schema successvol gecreëerd!";
                    else
                        TempData["error"] = "Schema kon niet gekoppeld/gecreëerd worden!";
                }
            }
            catch
            {
                TempData["error"] = "Inname creatie mislukt!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(PatientCreateModel model)
        {
            try
            {
                using (var client = new AuthHttpClient(User))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AdminToken"]);
                    var apiUri = new Uri(_apiURL + "/Patient");
                    var result = await client.PostAsJsonAsync(apiUri, model.PatientData);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await result.Content.ReadAsStringAsync();
                        var patientId = int.Parse(response);

                        using (var client2 = new HttpClient())
                        {
                            client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AdminToken"]);
                            var authUri = new Uri(_authURL + "/signup");

                            string json = JsonConvert.SerializeObject(new AuthRequestModel() //Creates a JSON object of the authRequest
                            {
                                UserName = model.AccountData.UserName,
                                Password = model.AccountData.Password,
                                Role = Roles.PatientOnly,
                                Id = patientId
                            }, Formatting.Indented);

                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            content.Headers.Remove("Content-Type");
                            content.Headers.Add("Content-Type", "application/json");

                            var authResult = await client2.PostAsync(authUri, content);

                            if (!authResult.IsSuccessStatusCode)
                            {


                                var deleteUri = new Uri(_apiURL + $"/Patient/{patientId}");
                                await client.DeleteAsync(deleteUri);

                                TempData["error"] = "Er is iets fout gegaan bij het registreren van het account!";
                            }
                            else
                            {
                                TempData["success"] = "Patiënt aangemaakt!";
                            }
                        }
                    }
                    else
                    {
                        TempData["error"] = "Er is iets fout gegaan bij het aanmaken van de Patiënt!";
                    }
                }
            }
            catch
            {
                TempData["error"] = "Patiënt aanmaken mislukt!";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: PatientController/Edit/id
        public async Task<ActionResult> Edit(int id)
        {
            if (id > 0)
            {
                try
                {
                    using (var client = new AuthHttpClient(User))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AdminToken"]);
                        var uri = new Uri(_apiURL + "/Patient/" + id);
                        var careWorkersUri = new Uri(_apiURL + "/Careworker");

                        var response = client.GetAsync(uri).Result;
                        var careWorkerseResponse = client.GetAsync(careWorkersUri).Result;

                        string result = await response.Content.ReadAsStringAsync();
                        string careWorkersResult = await careWorkerseResponse.Content.ReadAsStringAsync();

                        PatientModel? model = JsonConvert.DeserializeObject<PatientModel>(result);
                        ViewBag.CareWorkers = JsonConvert.DeserializeObject<List<CareWorkerModel>>(careWorkersResult);

                        return View(model);
                    }
                }
                catch
                {
                    TempData["error"] = "Ophalen van patiënt is mislukt!";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: PatientController/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(PatientModel model)
        {
            try
            {
                using (var client = new AuthHttpClient(User))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AdminToken"]);
                    var uri = new Uri(_apiURL + "/Patient/" + model.Id);
                    var result = await client.PutAsJsonAsync(uri, model);

                    TempData["success"] = "Patiënt is aangepast!";
                }
            }
            catch
            {
                TempData["error"] = "Er is iets fout gegaan bij het aanpassen van de Patiënt!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PatientController/Delete/id
        public async Task<ActionResult> Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    using (var client = new AuthHttpClient(User))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AdminToken"]);
                        var uri = new Uri(_apiURL + "/Patient/" + id);

                        var response = client.GetAsync(uri).Result;

                        string result = await response.Content.ReadAsStringAsync();

                        PatientModel? model = JsonConvert.DeserializeObject<PatientModel>(result);
                        return View(model);
                    }
                }
                catch
                {
                    TempData["error"] = "Ophalen van patiënt is mislukt!";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: PatientController/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id > 0)
            {
                try
                {
                    using (var client = new AuthHttpClient(User))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AdminToken"]);
                        var uri = new Uri(_apiURL + "/Patient/" + id);
                        var result = await client.DeleteAsync(uri);

                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                            TempData["success"] = "Patiënt successvol verwijderd!";
                        else
                            TempData["error"] = "Patiënt kon niet worden verwijderd!";
                    }
                }
                catch
                {
                    TempData["error"] = "Verwijderen mislukt!";
                }
            }
            return RedirectToAction(nameof(Index));
        }
        private bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
