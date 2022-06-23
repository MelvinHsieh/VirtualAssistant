using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class CareWorkerFunctionRepo : ICareWorkerFunctionRepo
    {
        private MedicineDbContext _context { get; set; }

        public CareWorkerFunctionRepo(MedicineDbContext context)
        {
            _context = context;
        }

        public Result AddCareWorkerFunction(string functionName)
        {
            if (string.IsNullOrEmpty(functionName))
            {
                return new Result(false, "Given function is empty!");
            }

            if (FindCareWorkerFunction(functionName) != null)
            {
                return new Result(false, "Given function already exists!");
            }

            CareWorkerFunction careWorkerFunction = new CareWorkerFunction()
            {
                Name = functionName
            };

            _context.CareWorkerFunctions.Add(careWorkerFunction);
            _context.SaveChanges();
            return new Result(true);
        }

        public CareWorkerFunction? FindCareWorkerFunction(string functionName)
        {
            return _context.CareWorkerFunctions.Find(functionName);
        }

        public List<CareWorkerFunction> GetCareWorkerFunctions()
        {
            return _context.CareWorkerFunctions.Where(x => x.Status == Domain.EntityStatus.Active.ToString().ToLower()).ToList();
        }

        public Result RemoveCareWorkerFunction(string functionName)
        {
            CareWorkerFunction? careWorkerFunction = FindCareWorkerFunction(functionName);
            if (careWorkerFunction == null)
            {
                return new Result(false, "No care worker function by the given name has been found");
            }

            careWorkerFunction.Status = Domain.EntityStatus.Archived.ToString().ToLower(); //Soft-Delete
            _context.CareWorkerFunctions.Update(careWorkerFunction);
            _context.SaveChanges();
            return new Result(true);
        }
    }
}
