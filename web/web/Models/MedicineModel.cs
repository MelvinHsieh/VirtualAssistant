using Newtonsoft.Json;
using System.ComponentModel;

namespace web.Models
{
    public class MedicineModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; } = 0;

        [JsonProperty(PropertyName = "name")]
        [DisplayName("Naam")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "indication")]
        [DisplayName("Indicatie")]
        public string Indication { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "dose")]
        [DisplayName("Dosering")]
        public double Dose { get; set; } = 0;

        [JsonProperty(PropertyName = "doseUnit")]
        [DisplayName("Dosering eenheid")]
        public string DoseUnit { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "shape")]
        [DisplayName("Vorm")]
        public string Shape { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "color")]
        [DisplayName("Kleur")]
        public string Color { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;
    }
}
