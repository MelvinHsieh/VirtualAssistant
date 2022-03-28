using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface IMedicineRepo
    {
        public Result AddMedicine(string name, string indicator, double dose, DoseUnit unit, MedicineType type, MedicineColor color, MedicineShape shape);

        public Medicine? GetMedicine(int id);

        public IEnumerable<Medicine> GetAllMedicine();

        public Result RemoveMedicine(int id);

        public Result ValidateMedicine(Medicine medicine);
    }
}
