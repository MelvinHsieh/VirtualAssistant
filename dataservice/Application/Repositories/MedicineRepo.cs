using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class MedicineRepo : IMedicineRepo
    {
        private MedicineDbContext _context { get; set; }
        private ITypeRepo _typeRepo { get; set; }
        private IColorRepo _colorRepo { get; set; }
        private IShapeRepo _shapeRepo { get; set; }
        private IDoseUnitRepo _unitRepo { get; set; }

        public MedicineRepo(MedicineDbContext context, IShapeRepo shapeRepo, IColorRepo colorRepo, ITypeRepo typeRepo, IDoseUnitRepo unitRepo)
        {
            _context = context;
            _shapeRepo = shapeRepo;
            _colorRepo = colorRepo;
            _typeRepo = typeRepo;
            _unitRepo = unitRepo;
        }

        public Result AddMedicine(string name, string indicator, double dose, string unit, string type, string color, string shape)
        {
            Medicine medicine = new Medicine()
            {
                Name = name,
                Indication = indicator,
                Dose = dose,
                DoseUnit = unit,
                Type = type,
                Color = color,
                Shape = shape
            };

            Result result = ValidateMedicine(medicine);

            if (result.Success == false)
            {
                return result;
            }

            _context.Medicine.Add(medicine);
            _context.SaveChanges();

            return new Result(true);
        }

        public Result RemoveMedicine(int id)
        {
            Medicine? medicine = GetMedicine(id);
            if (medicine == null)
            {
                return new Result(false, "No medicine with given Id exists.");
            }

            _context.Medicine.Remove(medicine);
            _context.SaveChanges();
            return new Result(true);
        }

        public IEnumerable<Medicine> GetAllMedicine()
        {
            return _context.Medicine;
        }

        public Medicine? GetMedicine(int id)
        {
            return _context.Medicine.Find(id);
        }

        public Result ValidateMedicine(Medicine medicine)
        {
            if (string.IsNullOrEmpty(medicine.Name))
            {
                return new Result(false, "A name is required");
            }

            if (!string.IsNullOrEmpty(medicine.DoseUnit))  //Dose unit is required, if present check for existence in db
            {
                if (_unitRepo.FindDoseUnit(medicine.DoseUnit) == null)
                {
                    return new Result(false, "Given dose unit does not exist in the system");
                }
            }
            else
            {
                return new Result(false, "A dose unit is required");
            }


            if (!string.IsNullOrEmpty(medicine.Color)) //Color is not required, if present check for existence in db 
            {
                if (_colorRepo.FindColor(medicine.Color) == null)
                {
                    return new Result(false, "Given color does not exist in the system");
                }
            }

            if (!string.IsNullOrEmpty(medicine.Shape)) //Shape is not required, if present check for existence in db
            {
                if (_shapeRepo.FindShape(medicine.Shape) == null)
                {
                    return new Result(false, "Given shape does not exist in the system");
                }
            }

            if (!string.IsNullOrEmpty(medicine.Type))  //Type is required, if present check for existence in db
            {
                if (_typeRepo.FindType(medicine.Type) == null)
                {
                    return new Result(false, "Given type does not exist in the system");
                }
            }
            else
            {
                return new Result(false, "A type is required");
            }

            //Validation Succesful
            return new Result(true);
        }
    }
}
