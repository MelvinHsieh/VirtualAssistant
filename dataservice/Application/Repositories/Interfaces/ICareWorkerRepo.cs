using Application.Common.Models;
using Domain.Entities.MedicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Interfaces
{
    public interface ICareWorkerRepo
    {
        public Result AddCareWorker(string firstName, string lastName, string function);

        public IEnumerable<CareWorker> GetCareWorkers();

        public CareWorker? FindCareWorker(int id);

        public Result RemoveCareWorker(int id);
    }
}
