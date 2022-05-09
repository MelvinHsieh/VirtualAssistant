using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface ITypeRepo
    {
        public Result AddType(string type);

        public IEnumerable<MedicineType> GetTypes();

        public MedicineType? FindType(string type);

        public Result RemoveType(string type);
    }
}
