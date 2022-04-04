using Newtonsoft.Json;

namespace web.Models
{
    public class MedicineModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; } = 0;
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "indication")]
        public string Indication { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "dose")]
        public double Dose { get; set; } = 0;
        [JsonProperty(PropertyName = "doseUnit")]
        public string DoseUnit { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "shape")]
        public string Shape { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;
    }
}
