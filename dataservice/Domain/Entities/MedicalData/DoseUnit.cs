using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MedicalData
{
    public class DoseUnit
    {
        public DoseUnit(string unit)
        {
            Unit = unit;
        }

        [Key]
        public string Unit { get; set; }
        public string Status { get; set; } = EntityStatus.Active.ToString().ToLower();
    }
}
