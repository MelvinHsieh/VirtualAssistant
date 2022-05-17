using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Enums
{
    public static class Roles
    {
        //Make sure these values match up with the authentication endpoint
        public const string
            AdminOnly = "admin",
            CareWorkerOnly = "zorgmedewerker",
            PatientOnly = "patient",
            All = $"{AdminOnly},{CareWorkerOnly},{PatientOnly}",
            Personnel = $"{AdminOnly},{CareWorkerOnly}";

    }
}
