using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MedicalData
{
    public class CareWorker
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = "";
        [Required]
        public string LastName { get; set; } = "";
        [Required]
        public string Function { get; set; } = "";
        public string Status { get; set; } = EntityStatus.Active.ToString().ToLower();
    }
}
