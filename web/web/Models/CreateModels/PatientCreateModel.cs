namespace web.Models.CreateModels
{
    public class PatientCreateModel
    {
        public RegisterModel AccountData { get; set; } = new RegisterModel();
        public PatientModel PatientData { get; set; } = new PatientModel();
    }
}
