using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MedicalData
{
    public class MedicineType
    {
        public MedicineType(string type)
        {
            Type = type;
        }

        [Key]
        public string Type { get; set; }
    }
}
