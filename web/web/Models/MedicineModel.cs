namespace web.Models
{
    public class MedicineModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Indication { get; set; } = string.Empty;
        public int Dose { get; set; } = 0;
        public string DoseUnit { get; set; } = string.Empty;
        public string Shape { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
