using Application.Repositories;
using Application.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineColorController : ControllerBase
    {
        private IColorRepo _colorRepo { get; set; }

        public MedicineColorController(IColorRepo colorRepo)
        {
            _colorRepo = colorRepo;
        }

        // GET: api/<MedicineColorController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _colorRepo.GetColors().ToList().Select(c => c.Color);
        }

        // GET api/<MedicineColorController>/red
        [HttpGet("{color}")]
        public string Get(string color)
        {
            return _colorRepo.FindColor(color).Color;
        }

        // POST api/<MedicineColorController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _colorRepo.AddColor(value);
        }

        // DELETE api/<MedicineColorController>/red
        [HttpDelete("{color}")]
        public void Delete(string color)
        {
            _colorRepo.RemoveColor(color);
        }
    }
}
