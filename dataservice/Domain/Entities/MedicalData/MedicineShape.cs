using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MedicalData
{
    public class MedicineShape
    {
        public MedicineShape(string shape)
        {
            Shape = shape;
        }

        [Key]
        public string Shape { get; set; }
        public string Status { get; set; } = EntityStatus.Active.ToString().ToLower();
    }
}
