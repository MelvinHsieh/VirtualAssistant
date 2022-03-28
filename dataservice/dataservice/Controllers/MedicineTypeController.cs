using Application.Repositories;
using Application.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineTypeController : ControllerBase
    {
        private ITypeRepo _typeRepo { get; set; }

        public MedicineTypeController(ITypeRepo typeRepo)
        {
            _typeRepo = typeRepo;
        }

        // GET: api/<MedicineTypeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _typeRepo.GetTypes().ToList().Select(c => c.Type);
        }

        // GET api/<MedicineTypeController>/pil
        [HttpGet("{type}")]
        public string Get(string type)
        {
            return _typeRepo.FindType(type).Type;
        }

        // POST api/<MedicineTypeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _typeRepo.AddType(value);
        }

        // DELETE api/<MedicineTypeController>/pil
        [HttpDelete("{type}")]
        public void Delete(string type)
        {
            _typeRepo.RemoveType(type);
        }
    }
}
