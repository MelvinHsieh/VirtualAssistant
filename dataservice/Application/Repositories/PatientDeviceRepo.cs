using Application.Common.Models;
using Application.Repositories.Interfaces;
using Domain.Entities.PatientData;
using Infrastructure.Persistence;

namespace Application.Repositories
{
    public class PatientDeviceRepo : IPatientDeviceRepo
    {
        private PatientDbContext _context;

        public PatientDeviceRepo(PatientDbContext context)
        {
            _context = context;
        }

        public Result SetNewActiveDevice(int patientId, string deviceId)
        {
            TryRemoveActiveDevice(patientId);

            _context.PatientDeviceIdentifiers.Add(new PatientDeviceIdentifier()
            {
                PatientId = patientId,
                DeviceId = deviceId
            });
            _context.SaveChanges();

            return new Result(true);
        }

        public Result TryRemoveActiveDevice(int patientId)
        {
            var activeDevice = _context.PatientDeviceIdentifiers
                .Where(p => p.PatientId == patientId)
                .Where(p => p.Status == Domain.EntityStatus.Active)
                .First();

            if (activeDevice != null)
            {
                activeDevice.Status = Domain.EntityStatus.Archived;

                _context.Update(activeDevice);
                _context.SaveChanges();

                return new Result(true);
            }

            return new Result(false, "No active device found");
        }

        public PatientDeviceIdentifier GetActiveDeviceForPatient(int patientId)
        {
            return _context.PatientDeviceIdentifiers
                .Where(p => p.PatientId == patientId)
                .Where(p => p.Status == Domain.EntityStatus.Active)
                .First();
        }

        public Dictionary<int, PatientDeviceIdentifier> GetActiveDeviceForPatient(int[] patientIdentifiers)
        {
            var dict = new Dictionary<int, PatientDeviceIdentifier>();
            foreach (var patient in patientIdentifiers)
            {
                dict.Add(patient,GetActiveDeviceForPatient(patient));
            }

            return dict;
        }
    }
}
