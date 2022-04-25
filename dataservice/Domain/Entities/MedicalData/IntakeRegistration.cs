using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MedicalData
{
    public class IntakeRegistration
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public int IntakeId { get; set; }
        [Required]
        public string Status { get; set; } = EntityStatus.Active.ToString().ToLower();
    }
}
