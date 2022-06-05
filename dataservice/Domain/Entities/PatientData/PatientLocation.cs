using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PatientData
{
    public class PatientLocation
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Department { get; set; } = "TestLocatie";
        
        [Required]
        public string RoomId { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = EntityStatus.Active.ToString().ToLower();

        public int PatientId { get; set; } = 0;

        public virtual Patient? Patient { get; set; }

    }
}
