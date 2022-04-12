namespace Domain.Entities.MedicalData
{
    public class MedicineType
    {
        public MedicineType(string type)
        {
            Type = type;
        }
        public string Type { get; set; }
    }
}
