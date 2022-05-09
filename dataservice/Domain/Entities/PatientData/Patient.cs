using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.PatientData
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public string? PostalCode { get; set; }

        public string? HomeNumber { get; set; }

        public string Status { get; set; } = EntityStatus.Active.ToString().ToLower();
    }
}
