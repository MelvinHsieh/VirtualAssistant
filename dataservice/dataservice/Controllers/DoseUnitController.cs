using Application.Repositories;
using Application.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoseUnitController : ControllerBase
    {
        private IDoseUnitRepo _doseUnitRepo { get; set; }

        public DoseUnitController(IDoseUnitRepo doseUnitRepo)
        {
            _doseUnitRepo = doseUnitRepo;
        }

        // GET: api/<MedicineDoseUnitController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _doseUnitRepo.GetDoseUnits().ToList().Select(c => c.Unit);
        }

        // GET api/<MedicineDoseUnitController>/mg
        [HttpGet("{doseUnit}")]
        public string Get(string doseUnit)
        {
            return _doseUnitRepo.FindDoseUnit(doseUnit).Unit;
        }

        // POST api/<MedicineDoseUnitController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _doseUnitRepo.AddDoseUnit(value);
        }

        // DELETE api/<MedicineDoseUnitController>/mg
        [HttpDelete("{doseUnit}")]
        public void Delete(string doseUnit)
        {
            _doseUnitRepo.RemoveDoseUnit(doseUnit);
        }
    }
}
