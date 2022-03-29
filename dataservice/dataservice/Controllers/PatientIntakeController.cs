using Application.Repositories.Interfaces;
using dataservice.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientIntakeController : ControllerBase
    {
        private IPatientIntakeRepo _intakeRepo;

        public PatientIntakeController(IPatientIntakeRepo intakeRepo)
        {
            _intakeRepo = intakeRepo;
        }

        // GET: api/<PatientIntakeController>
        /*        [HttpGet]
                public IEnumerable<string> Get()
                {
                    return new string[] { "value1", "value2" };
                }*/

        // GET api/<PatientIntakeController>/5
        [HttpGet("/intake/{id}")]
        public IActionResult GetByIntakeId(int id)
        {
            var intake = _intakeRepo.GetIntakeById(id);
            if (intake == null)
            {
                return NotFound();
            }

            return Ok(intake);
        }

        // GET api/<PatientIntakeController>/5
        [HttpGet("/patient/{id}")]
        public IActionResult GetByPatientId(int id)
        {
            var intake = _intakeRepo.GetIntakesByPatientId(id);
            if (intake == null)
            {
                return NotFound();
            }

            return Ok(intake);
        }

        // POST api/<PatientIntakeController>
        [HttpPost]
        public void Post([FromBody] IntakeDto data)
        {
            if (data != null)
            {
                TimeOnly start;
                TimeOnly end;
                if (TimeOnly.TryParse(data.IntakeStart, out start) || TimeOnly.TryParse(data.IntakeEnd, out end))
                {
                    _intakeRepo.AddIntake(data.MedicineId, data.PatientId, data.Amount, start, end);
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
            _intakeRepo.RemoveIntake(id);
        }
    }
}
