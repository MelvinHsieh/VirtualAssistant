using Application.Common.Models;
using Domain.Entities.MedicalData;

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
