namespace dataservice.DTO
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Indication { get; set; }
        public double Dose { get; set; }
        public string? DoseUnit { get; set; }
        public string? Shape { get; set; }
        public string? Color { get; set; }
        public string? Type { get; set; }
    }
}
