using Application.Common.Models;
using Domain.Entities.MedicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Interfaces
{
    public interface ICareWorkerFunctionRepo
    {
        public Result AddCareWorkerFunction(string functionName);

        public IEnumerable<CareWorkerFunction> GetCareWorkerFunctions();

        public CareWorkerFunction? FindCareWorkerFunction(string functionName);

        public Result RemoveCareWorkerFunction(string functionName);
    }
}
