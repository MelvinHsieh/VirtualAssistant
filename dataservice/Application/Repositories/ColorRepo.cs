using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.MedicalData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class ColorRepo : IColorRepo
    {
        private MedicineDbContext _context { get; set; }

        public ColorRepo(MedicineDbContext context)
        {
            _context = context;
        }

        public Result AddColor(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                return new Result(false, "Given color name is null or empty");
            }

            MedicineColor? medicineColor = FindColor(color);
            if (medicineColor != null)
            {
                return new Result(false, "The given color already exists");
            }

            _context.Medicine_Colors.Add(new MedicineColor(color));
            _context.SaveChanges();
            return new Result(true);
        }

        public MedicineColor? FindColor(string color)
        {
            return _context.Medicine_Colors.Find(color);
        }

        public IEnumerable<MedicineColor> GetColors()
        {
            return _context.Medicine_Colors;
        }

        public Result RemoveColor(string color)
        {
            MedicineColor? medicineColor = FindColor(color);
            if (medicineColor == null)
            {
                return new Result(false, "No color by the given name has been found");
            }

            _context.Medicine_Colors.Remove(medicineColor);
            _context.SaveChanges();
            return new Result(true);
        }
    }
}
