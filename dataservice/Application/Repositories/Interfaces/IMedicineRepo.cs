using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface IMedicineRepo
    {
        public Result AddMedicine(string name, string indicator, double dose, string unit, string type, string color, string shape);

        public Medicine? GetMedicine(int id);

        public IEnumerable<Medicine> GetAllMedicine();

        public Result RemoveMedicine(int id);

        public Result ValidateMedicine(Medicine medicine);
    }
}
