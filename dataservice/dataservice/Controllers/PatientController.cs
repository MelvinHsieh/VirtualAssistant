using Application.Common.Enums;
using Application.Common.Models;
using Application.Repositories.Interfaces;
using dataservice.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Personnel)]
    public class PatientController : ControllerBase
    {
        private IPatientRepo _patientRepo;

        public PatientController(IPatientRepo patientRepo)
        {
            _patientRepo = patientRepo;
        }

        // GET: api/<PatientController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_patientRepo.GetAllPatients());
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var patient = _patientRepo.GetPatient(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // POST api/<PatientController>
        [HttpPost]
        public int? Post([FromBody] PatientDto data)
        {
            if (data != null)
            {
                DateTime date;
                if (DateTime.TryParse(data.BirthDate, out date))
                {
                    var result = _patientRepo.AddPatient(data.FirstName, data.LastName, date, data.PostalCode, data.HomeNumber, data.Email, data.PhoneNumber, data.RoomId);
                    return (int?)result.ResponseData;
                }
            }
            return null;
        }

        //POST api/<PatientController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PatientDto data)
        {
            Result result = new Result(false);
            if (data != null)
            {
                DateTime date;
                if (DateTime.TryParse(data.BirthDate, out date))
                {
                    result = _patientRepo.UpdatePatient(id, data.FirstName, data.LastName, date, data.PostalCode, data.HomeNumber, data.Email, data.PhoneNumber, data.CareWorkerId, data.RoomId);
                }
            }

            return Ok(result);
        }

        // DELETE api/<PatientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _patientRepo.RemovePatient(id);
        }
    }
}
