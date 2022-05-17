using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using web.Models;
using web.Utils;

namespace web.Controllers
{
    public class PatientController : Controller
    {

        private readonly string _apiURL;

        public PatientController(IConfiguration configuration)
        {
            _apiURL = configuration.GetValue<String>("DataServiceURL");
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
        public async Task<ActionResult> CreateAsync(PatientModel model)
        {
            try
            {
                using (var client = new AuthHttpClient(User))
                {
                    var uri = new Uri(_apiURL + "/Patient");
                    var result = await client.PostAsJsonAsync(uri, model);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["success"] = "Patiënt aangemaakt!";
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

        // POST: PatientController/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id)
        {
            if (id > 0)
            {
                try
                {
                    using (var client = new AuthHttpClient(User))
                    {
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

        // GET: PatientController/Delete/id
        public async Task<ActionResult> Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    using (var client = new AuthHttpClient(User))
                    {
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
        {//Using JSON.Net
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
