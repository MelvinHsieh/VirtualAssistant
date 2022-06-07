using Application.Repositories.Interfaces;
using Domain.Entities.PatientData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientDeviceController : ControllerBase
    {
        private IPatientDeviceRepo _patientDeviceRepo; 

        public PatientDeviceController(IPatientDeviceRepo patientDeviceRepo)
        {
            _patientDeviceRepo = patientDeviceRepo;
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var found = _patientDeviceRepo.GetActiveDeviceForPatient(id);

            if(found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        // POST api/<PatientController>
        [HttpPost]
        public IActionResult Post([FromBody] PatientDeviceIdentifier data)
        {
            if (data != null)
            {
                if (data.DeviceId != null && data.PatientId != 0)
                {
                    _patientDeviceRepo.SetNewActiveDevice(data.PatientId, data.DeviceId);

                    return Ok();
                }  
            }

            return BadRequest();
        }

        // DELETE api/<PatientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _patientDeviceRepo.TryRemoveActiveDevice(id);
        }
    }
}
