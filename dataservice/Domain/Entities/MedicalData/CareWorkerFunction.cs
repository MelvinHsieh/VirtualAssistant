using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MedicalData
{
    public class CareWorkerFunction
    {
        [Key]
        public string Name { get; set; }
        public string Status { get; set; } = EntityStatus.Active.ToString().ToLower();
    }
}
