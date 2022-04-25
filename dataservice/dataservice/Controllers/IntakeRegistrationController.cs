using Application.Repositories.Interfaces;
using dataservice.ViewModels;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace dataservice.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class IntakeRegistrationController : ControllerBase
    {
        private IIntakeRegistrationRepo _registrationRepo;
        private JsonSerializerOptions _jserOptions;

        public IntakeRegistrationController(IIntakeRegistrationRepo repo)
        {
            _registrationRepo = repo;
            _jserOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            _jserOptions.Converters.Add(new DateOnlySerializer());
        }

        // GET: api/<PatientIntakeController>
        /*        [HttpGet]
                public IEnumerable<string> Get()
                {
                    return new string[] { "value1", "value2" };
                }*/

        // GET api/<PatientIntakeController>/5
        [HttpGet("intake/{id}")]
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

        // GET api/<PatientIntakeController>/5
        /*[HttpGet("patient/{id}")] TODO
        public IActionResult GetByPatientId(int id)
        {

            var intake = _registrationRepo.GetIntakeRegistrationForDate(id);
            if (intake == null)
            {
                return NotFound();
            }

            var result = JsonSerializer.Serialize(intake, _jserOptions);

            return Ok(result);

        }*/

        // POST api/<PatientIntakeController>
        [HttpPost]
        public void Post([FromBody] IntakeRegistrationDto data)
        {
            if (data != null)
            {
                DateOnly date;
                if (DateOnly.TryParse(data.Date, out date))
                {
                    _registrationRepo.AddIntakeRegistration(date, data.IntakeId);
                }
            }
        }

        // PUT api/<PatientIntakeController>/5 NO EDIT FUNTIONALITY YET

        /*        [HttpPut("{id}")]
                public void Put(int id, [FromBody] string value)
                {
                }*/

        // DELETE api/<PatientIntakeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _registrationRepo.RemoveIntakeRegistration(id);
        }
    }
}
