using Application.Common.Models;
using Domain.Entities.MedicalData;

namespace Application.Repositories.Interfaces
{
    public interface IColorRepo
    {
        public Result AddColor(string color);

        public IEnumerable<MedicineColor> GetColors();

        public MedicineColor? FindColor(string color);

        public Result RemoveColor(string color);
    }
}
