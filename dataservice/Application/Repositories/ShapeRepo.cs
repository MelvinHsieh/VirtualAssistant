using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class ShapeRepo : IShapeRepo
    {
        private MedicineDbContext _context { get; set; }

        public ShapeRepo(MedicineDbContext context)
        {
            _context = context;
        }

        public Result AddShape(string shape)
        {
            if (string.IsNullOrEmpty(shape))
            {
                return new Result(false, "Given shape name is null or empty");
            }

            MedicineShape? medicineShape = FindShape(shape);
            if (medicineShape != null)
            {
                return new Result(false, "The given shape already exists");
            }

            _context.Medicine_Shapes.Add(new MedicineShape(shape));
            _context.SaveChanges();
            return new Result(true);
        }

        public MedicineShape? FindShape(string shape)
        {
            return _context.Medicine_Shapes.Find(shape);
        }

        public IEnumerable<MedicineShape> GetShapes()
        {
            return _context.Medicine_Shapes;
        }

        public Result RemoveShape(string shape)
        {
            MedicineShape? medicineShape = FindShape(shape);
            if (medicineShape == null)
            {
                return new Result(false, "No shape by the given name has been found");
            }

            _context.Medicine_Shapes.Remove(medicineShape);
            _context.SaveChanges();
            return new Result(true);
        }
    }
}
