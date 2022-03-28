using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface IDoseUnitRepo
    {
        public Result AddDoseUnit(string unit);

        public IEnumerable<DoseUnit> GetDoseUnits();

        public DoseUnit? FindDoseUnit(string unit);

        public Result RemoveDoseUnit(string unit);
    }
}
