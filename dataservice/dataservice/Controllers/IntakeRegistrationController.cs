using Application.Common.Enums;
using Application.Repositories.Interfaces;
using dataservice.DTO;
using Domain.Entities.MedicalData;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace dataservice.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.All)]
    /*
     * GET intakeRegistration -> All Roles
     * CREATE intakeRegistration -> All Roles
     * DELETE intakeRegistration -> All Roles
     */
    public class IntakeRegistrationController : ControllerBase
    {
        private IIntakeRegistrationRepo _registrationRepo;
        private JsonSerializerOptions _jserOptions;

        public IntakeRegistrationController(IIntakeRegistrationRepo repo)
        {
            _registrationRepo = repo;
            _jserOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            _jserOptions.Converters.Add(new DateOnlySerializer());
            _jserOptions.Converters.Add(new TimeOnlySerializer());
        }

        // GET api/<IntakeRegistrationController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.All)]
        public IActionResult GetByRegistrationId(int id)
        {
            try
            {
                var intakeRegistration = _registrationRepo.GetIntakeRegistration(id);
                if (intakeRegistration == null)
                {
                    return NotFound();
                }

                var result = JsonSerializer.Serialize(intakeRegistration, _jserOptions);

                return Ok(result);
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex);
            }

        }

        // GET api/<PatientIntakeController>/patient/5?date=21/04/2012
        [HttpGet("patient/{patientId}")]
        public IActionResult GetByPatientId(int patientId, [FromQuery] string? date)
        {
            IEnumerable<IntakeRegistration> intake = new List<IntakeRegistration>();
            if (date != null)
            {
                DateOnly parseDate;
                if (DateOnly.TryParse(date, out parseDate))
                {
                    intake = _registrationRepo.GetIntakeRegistrationForDate(patientId, parseDate);
                }
            }
            else
            {
                intake = _registrationRepo.GetIntakeRegistrationForPatient(patientId);
            }

            var result = JsonSerializer.Serialize(intake, _jserOptions);

            return Ok(result);

        }


        // POST api/<IntakeRegistrationController>
        [HttpPost]
        public void Post([FromBody] IntakeRegistrationDto data)
        {
            if (data != null)
            {
                DateTime date;
                if (DateTime.TryParse(data.Date, out date))
                {
                    _registrationRepo.AddIntakeRegistration(date, data.PatientIntakeId);
                }
            }
        }

        // PUT api/<IntakeRegistrationController>/5 NO EDIT FUNTIONALITY YET

        /*        [HttpPut("{id}")]
                public void Put(int id, [FromBody] string value)
                {
                }*/

        // DELETE api/<IntakeRegistrationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _registrationRepo.RemoveIntakeRegistration(id);
        }
    }
}
