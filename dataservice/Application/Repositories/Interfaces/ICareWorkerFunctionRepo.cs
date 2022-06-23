using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface ICareWorkerFunctionRepo
    {
        public Result AddCareWorkerFunction(string functionName);

        public List<CareWorkerFunction> GetCareWorkerFunctions();

        public CareWorkerFunction? FindCareWorkerFunction(string functionName);

        public Result RemoveCareWorkerFunction(string functionName);
    }
}
