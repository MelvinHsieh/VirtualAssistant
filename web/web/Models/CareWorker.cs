using System.ComponentModel.DataAnnotations;

namespace web.Models
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
    }
}
