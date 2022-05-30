using Application.Repositories.Interfaces;
using dataservice.DTO;
using Microsoft.AspNetCore.Mvc;

namespace dataservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CareWorkerController : ControllerBase
    {
        private ICareWorkerRepo _careWorkerRepo { get; set; }
        private ICareWorkerFunctionRepo _careWorkerFunctionRepo { get; set; }
        public CareWorkerController(ICareWorkerRepo careWorkerRepo, ICareWorkerFunctionRepo careWorkerFunctionRepo)
        {
            _careWorkerRepo = careWorkerRepo;
            _careWorkerFunctionRepo = careWorkerFunctionRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_careWorkerRepo.GetCareWorkers());
        }

        // GET api/<PatientIntakeController>/functions
        [HttpGet("function")]
        public IEnumerable<string> GetFunctions()
        {
            return _careWorkerFunctionRepo.GetCareWorkerFunctions().ToList().Select(c => c.Name);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_careWorkerRepo.FindCareWorker(id));
        }

        // POST api/<ValuesController>
        [HttpPost]
        public int? Post([FromBody] CareWorkerDto data)
        {
            if (data != null)
            {
                var result = _careWorkerRepo.AddCareWorker(data.FirstName, data.LastName, data.Function);
                return (int?)result.ResponseData;
            }
            return null;
        }

        [HttpPost("function")]
        public void PostCareWorkerFunction([FromBody] string value)
        {
            _careWorkerFunctionRepo.AddCareWorkerFunction(value);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _careWorkerRepo.RemoveCareWorker(id);
        }

        // DELETE api/<MedicineColorController>/function/red
        [HttpDelete("function/{function}")]
        public void Delete(string function)
        {
            _careWorkerFunctionRepo.RemoveCareWorkerFunction(function);
        }
    }
}
