using Application.Common.Models;
using Application.Repositories.Interfaces;
using dataservice.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientEmergencyController : ControllerBase
    {
        private IPatientRepo _patientRepo;

        public PatientEmergencyController(IPatientRepo patientRepo)
        {
            _patientRepo = patientRepo;
        }

        /// <summary>
        /// Register an alert.
        /// </summary>
        // POST api/<PatientController>
        [HttpPost]
        public IActionResult Post([FromBody] EmergencyDto data)
        {
            Result result = null;

            if (data != null)
            {
                int patientId = data.PatientId;

                result = _patientRepo.RegisterAlert(patientId);
            }

            return Ok(result);
        }

        [HttpPost("/confirm")]
        public IActionResult Confirm([FromBody] int emergencyNoticeId)
        {
            Result result = null;

            return Ok(result);
        }
    }
}
