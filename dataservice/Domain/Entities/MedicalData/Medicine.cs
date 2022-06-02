using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MedicalData
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Indication { get; set; }
        [Required]
        public double Dose { get; set; }
        [Required]
        public string? DoseUnit { get; set; }
        public string? Shape { get; set; }
        public string? Color { get; set; }
        [Required]
        public string? Type { get; set; }
        public string? ImageURL { get; set; }
        public string Status { get; set; } = EntityStatus.Active.ToString().ToLower();
    }
}
