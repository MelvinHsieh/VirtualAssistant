using Application.Repositories.Interfaces;
using dataservice.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private IMedicineRepo _medicineRepo;

        public MedicineController(IMedicineRepo medicineRepo)
        {
            _medicineRepo = medicineRepo;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_medicineRepo.GetAllMedicine());
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_medicineRepo.GetMedicine(id));
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] MedicineDto data)
        {
            if (data != null)
            {
                _medicineRepo.AddMedicine(data.Name, data.Indication, data.Dose, data.DoseUnit, data.Type, data.Color, data.Shape);
            }
        }

        // PUT api/<ValuesController>/5 NO EDIT FUNCTIONALITY
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }*/

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _medicineRepo.RemoveMedicine(id);
        }
    }
}
