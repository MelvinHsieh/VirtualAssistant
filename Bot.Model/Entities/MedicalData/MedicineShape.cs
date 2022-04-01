namespace Domain.Entities.MedicalData
{
    public class MedicineShape
    {
        public MedicineShape(string shape)
        {
            Shape = shape;
        }

        public string Shape { get; set; }
    }
}
