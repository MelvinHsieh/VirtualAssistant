namespace Domain.Entities.MedicalData
{
    public class DoseUnit
    {
        public DoseUnit(string unit)
        {
            Unit = unit;
        }

        public string Unit { get; set; }
    }
}
