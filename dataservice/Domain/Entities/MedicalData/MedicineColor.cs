using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MedicalData
{
    public class MedicineColor
    {
        public MedicineColor(string color)
        {
            Color = color;
        }

        [Key]
        public string Color { get; set; }
    }
}
