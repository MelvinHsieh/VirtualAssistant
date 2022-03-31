using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface IShapeRepo
    {
        public Result AddShape(string shape);

        public IEnumerable<MedicineShape> GetShapes();

        public MedicineShape? FindShape(string shape);

        public Result RemoveShape(string shape);
    }
}
