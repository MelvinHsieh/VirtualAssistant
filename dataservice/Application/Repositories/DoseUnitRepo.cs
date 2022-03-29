using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class DoseUnitRepo : IDoseUnitRepo
    {
        private MedicineDbContext _context { get; set; }

        public DoseUnitRepo(MedicineDbContext context)
        {
            _context = context;
        }

        public Result AddDoseUnit(string unit)
        {
            if (string.IsNullOrEmpty(unit))
            {
                return new Result(false, "Given dose unit name is null or empty");
            }

            DoseUnit? doseUnit = FindDoseUnit(unit);
            if (doseUnit != null)
            {
                return new Result(false, "The given dose unit already exists");
            }

            _context.DoseUnits.Add(new DoseUnit(unit));
            _context.SaveChanges();
            return new Result(true);
        }

        public DoseUnit? FindDoseUnit(string unit)
        {
            return _context.DoseUnits.Find(unit);
        }

        public IEnumerable<DoseUnit> GetDoseUnits()
        {
            return _context.DoseUnits;
        }

        public Result RemoveDoseUnit(string unit)
        {
            DoseUnit? doseUnit = FindDoseUnit(unit);
            if (doseUnit == null)
            {
                return new Result(false, "No dose unit by the given name has been found");
            }

            _context.DoseUnits.Remove(doseUnit);
            _context.SaveChanges();
            return new Result(true);
        }
    }
}
