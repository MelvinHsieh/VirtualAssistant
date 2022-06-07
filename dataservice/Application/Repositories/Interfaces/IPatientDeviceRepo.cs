using Application.Common.Models;
using Domain.Entities.PatientData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.Interfaces
{
    public interface IPatientDeviceRepo
    {
        public Result SetNewActiveDevice(int patientId, string deviceId);
        public Result TryRemoveActiveDevice(int patientId);
        public PatientDeviceIdentifier GetActiveDeviceForPatient(int patientId);
        public Dictionary<int, PatientDeviceIdentifier> GetActiveDeviceForPatient(int[] patientIdentifiers);

    }
}
