namespace dataservice.ViewModels
{
    public class IntakeDto
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int MedicineId { get; set; }

        public string IntakeStart { get; set; }

        public string IntakeEnd { get; set; }

        public int Amount { get; set; }
    }
}
