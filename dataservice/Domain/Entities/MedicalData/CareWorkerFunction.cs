using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MedicalData
{
    public class CareWorkerFunction
    {
        [Required]
        [Key]
        public string Name { get; set; } = "";
    }
}
