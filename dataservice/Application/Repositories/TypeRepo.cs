using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class TypeRepo : ITypeRepo
    {
        private MedicineDbContext _context { get; set; }

        public TypeRepo(MedicineDbContext context)
        {
            _context = context;
        }

        public Result AddType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return new Result(false, "Given type name is null or empty");
            }

            MedicineType? medicineType = FindType(type);
            if (medicineType != null)
            {
                return new Result(false, "The given type already exists");
            }

            _context.Medicine_Types.Add(new MedicineType(type));
            _context.SaveChanges();
            return new Result(true);
        }

        public MedicineType? FindType(string type)
        {
            return _context.Medicine_Types.Find(type);
        }

        public IEnumerable<MedicineType> GetTypes()
        {
            return _context.Medicine_Types;
        }

        public Result RemoveType(string type)
        {
            MedicineType? medicineType = FindType(type);
            if (medicineType == null)
            {
                return new Result(false, "No type by the given name has been found");
            }

            _context.Medicine_Types.Remove(medicineType);
            _context.SaveChanges();
            return new Result(true);
        }
    }
}
