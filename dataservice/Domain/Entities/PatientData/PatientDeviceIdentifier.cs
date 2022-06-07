
namespace Domain.Entities.PatientData
{
    public class PatientDeviceIdentifier
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? DeviceId { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
    }
}
