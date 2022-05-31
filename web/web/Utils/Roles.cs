namespace web.Utils
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
