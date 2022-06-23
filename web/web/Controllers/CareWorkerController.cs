using Microsoft.AspNetCore.Mvc;
﻿using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;
using web.Models;
using web.Models.Common;
using web.Models.CreateModels;
using web.Utils;

namespace web.Controllers
{
    [Authorize(Roles = Roles.Personnel)]
    public class CareWorkerController : Controller
    {
        private readonly string _apiURL;
        private readonly string _authURL;

        public CareWorkerController(IConfiguration configuration)
        {
            _apiURL = configuration.GetValue<String>("DataServiceURL");
            _authURL = configuration.GetValue<String>("AuthURL");
        }

        // GET: CareWorkerController
        public async Task<ActionResult> IndexAsync()
        {
            using (var client = new AuthHttpClient(User))
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
        [Authorize(Roles = Roles.AdminOnly)]
        public async Task<ActionResult> CreateAsync()
        {
            using (var client = new AuthHttpClient(User))
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
        [Authorize(Roles = Roles.AdminOnly)]
        public async Task<ActionResult> CreateAsync(CareWorkerCreateModel model)
        {
            try
            {
                using (var client = new AuthHttpClient(User))
                {
                    var apiUri = new Uri(_apiURL + "/CareWorker");
                    var result = await client.PostAsJsonAsync(apiUri, model.CareWorkerData);


                    if (result.IsSuccessStatusCode)
                    {
                        var response = await result.Content.ReadAsStringAsync();
                        var careWorkerId = int.Parse(response);

                        using (var client2 = new HttpClient())
                        {
                            var authUri = new Uri(_authURL + "/signup");

                            string json = JsonConvert.SerializeObject(new AuthRequestModel() //Creates a JSON object of the authRequest
                            {
                                UserName = model.AccountData.UserName,
                                Password = model.AccountData.Password,
                                Role = Roles.CareWorkerOnly,
                                Id = careWorkerId
                            }, Formatting.Indented);

                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            content.Headers.Remove("Content-Type");
                            content.Headers.Add("Content-Type", "application/json");

                            var authResult = await client2.PostAsync(authUri, content);

                            if (!authResult.IsSuccessStatusCode)
                            {


                                var deleteUri = new Uri(_apiURL + $"/CareWorker/{careWorkerId}");
                                await client.DeleteAsync(deleteUri);

                                TempData["error"] = "Er is iets fout gegaan bij het registreren van het account!";
                            }
                            else
                            {
                                TempData["success"] = "Zorgmedewerker aangemaakt!";
                            }
                        }
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
        [Authorize(Roles = Roles.AdminOnly)]
        public ActionResult CreateCareWorkerFunction()
        {
            return View();
        }

        // POST: CareWorkerController/CreateCareWorkerFunction
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.AdminOnly)]
        public async Task<ActionResult> CreateCareWorkerFunctionAsync(string careWorkerFunction)
        {
            try
            {
                using (var client = new AuthHttpClient(User))
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
        [Authorize(Roles = Roles.AdminOnly)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id != 0)
            {
                try
                {
                    using (var client = new AuthHttpClient(User))
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
