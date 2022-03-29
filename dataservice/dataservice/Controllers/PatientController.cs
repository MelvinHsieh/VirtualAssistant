using Application.Repositories.Interfaces;
using dataservice.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private IPatientRepo _patientRepo;

        public PatientController(IPatientRepo patientRepo)
        {
            _patientRepo = patientRepo;
        }

        // GET: api/<PatientController>
        [HttpGet]
        public string Get()
        {
            return "Cannot give out patient data";
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var patient = _patientRepo.GetPatient(id);
            if (patient != null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // POST api/<PatientController>
        [HttpPost]
        public void Post([FromBody] PatientDto data)
        {
            if (data != null)
            {
                DateTime date;
                if (DateTime.TryParse(data.BirthDate, out date))
                {
                    _patientRepo.AddPatient(data.FirstName, data.LastName, date, data.PostalCode, data.HomeNumber, data.Email, data.PhoneNumber);
                }
            }
        }

        // PUT api/<PatientController>/5 NO EDIT FUNCTIONALITY YET
        /*     [HttpPut("{id}")]
             public void Put(int id, [FromBody] string value)
             {
             }*/

        // DELETE api/<PatientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _patientRepo.RemovePatient(id);
        }
    }
}
