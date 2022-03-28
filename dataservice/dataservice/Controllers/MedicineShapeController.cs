using Application.Repositories;
using Application.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineShapeController : ControllerBase
    {
        private IShapeRepo _shapeRepo { get; set; }

        public MedicineShapeController(IShapeRepo shapeRepo)
        {
            _shapeRepo = shapeRepo;
        }

        // GET: api/<MedicineShapeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _shapeRepo.GetShapes().ToList().Select(c => c.Shape);
        }

        // GET api/<MedicineShapeController>/vierkant
        [HttpGet("{shape}")]
        public string Get(string shape)
        {
            return _shapeRepo.FindShape(shape).Shape;
        }

        // POST api/<MedicineShapeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _shapeRepo.AddShape(value);
        }

        // DELETE api/<MedicineShapeController>/vierkant
        [HttpDelete("{shape}")]
        public void Delete(string shape)
        {
            _shapeRepo.RemoveShape(shape);
        }
    }
}
