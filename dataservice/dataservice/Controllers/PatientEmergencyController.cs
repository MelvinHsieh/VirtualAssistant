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
        public void Post([FromBody] EmergencyDto data)
        {
            if (data != null)
            {
                DateTime date;
                int patientId = data.PatientId;
                if (DateTime.TryParse(data.date, out date))
                {
                    _patientRepo.RegisterAlert(patientId, date);
                }
            }
        }
        
    }
}
