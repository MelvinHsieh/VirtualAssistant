using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class CareWorkerRepo : ICareWorkerRepo
    {
        private MedicineDbContext _context { get; set; }
        private ICareWorkerFunctionRepo _careWorkerFunctionRepo { get; set; }


        public CareWorkerRepo(MedicineDbContext context)
        {
            _context = context;
        }

        public Result AddCareWorker(string firstName, string lastName, string functionName)
        {
            if (string.IsNullOrEmpty(firstName) || 
                string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(functionName))
            {
                return new Result(false, "Given firstname, lastname or function was empty!");
            }

            if (_careWorkerFunctionRepo.FindCareWorkerFunction(functionName) == null)
            {
                return new Result(false, "Given care worker function does not exist!");
            }

            CareWorker careWorker = new CareWorker()
            {
                FirstName = firstName,
                LastName = lastName,
                Function = functionName
            };

            _context.CareWorkers.Add(careWorker);
            _context.SaveChanges();
            return new Result(true);
        }

        public CareWorker? FindCareWorker(int id)
        {
            return _context.CareWorkers.Find(id);
        }

        public IEnumerable<CareWorker> GetCareWorkers()
        {
            return _context.CareWorkers.Where(x => x.Status == Domain.EntityStatus.Active.ToString().ToLower());
        }

        public Result RemoveCareWorker(int id)
        {
            CareWorker? careWorker = FindCareWorker(id);
            if (careWorker == null)
            {
                return new Result(false, "No care worker found by this id!");
            }

            careWorker.Status = Domain.EntityStatus.Archived.ToString().ToLower(); //Soft-Delete
            _context.CareWorkers.Update(careWorker);
            _context.SaveChanges();
            return new Result(true);
        }
    }
}
