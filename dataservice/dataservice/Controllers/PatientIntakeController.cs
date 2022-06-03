using Application.Common.Enums;
using Application.Repositories.Interfaces;
using dataservice.DTO;
using Domain.Entities.MedicalData;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Personnel)]
    /*
     * GET intake -> All Roles
     * CREATE intake -> Personnel only
     * DELETE intake -> Personnel only
     */
    public class PatientIntakeController : ControllerBase
    {
        private IPatientIntakeRepo _intakeRepo;
        private JsonSerializerOptions _jserOptions;

        public PatientIntakeController(IPatientIntakeRepo intakeRepo)
        {
            _intakeRepo = intakeRepo;
            _jserOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            _jserOptions.Converters.Add(new TimeOnlySerializer());
        }

        // GET api/<PatientIntakeController>/5
        [HttpGet("intake/{id}")]
        [Authorize(Roles = Roles.All)]
        public IActionResult GetByIntakeId(int id)
        {
            try
            {
                var intake = _intakeRepo.GetIntakeById(id);
                if (intake == null)
                {
                    return NotFound();
                }

                var result = JsonSerializer.Serialize(intake, _jserOptions);

                return Ok(result);
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex);
            }

        }

        // GET api/<PatientIntakeController>/5
        [HttpGet("patient/{id}")]
        [Authorize(Roles = Roles.All)]
        public IActionResult GetByPatientId(int id)
        {
           
            var intake = _intakeRepo.GetRemainingIntakesByPatientId(id);
            
            if (intake.Count() <= 0)
            {
                return NotFound();
            }

            var result = JsonSerializer.Serialize(intake, _jserOptions);

            return Ok(result);

        }

        [HttpGet("missed")]
        [Authorize(Roles = Roles.AdminOnly)]
        public IActionResult GetAllMissedIntakes([FromQuery] DateTime searchStart, [FromQuery] DateTime searchEnd)
        {
            var intake = _intakeRepo.GetAllMissedIntakes(searchStart, searchEnd);

            if(intake.Count() <= 0)
            {
                return Ok("");
            }
            var result = JsonSerializer.Serialize(intake, _jserOptions);

            return Ok(result);
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
