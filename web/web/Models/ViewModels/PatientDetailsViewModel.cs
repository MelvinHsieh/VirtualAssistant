namespace web.Models.ViewModels
{
    public class PatientDetailsViewModel
    {
        public PatientModel? Patient { get; set; }
        public List<IntakeModel>? Intake { get; set; }

        public PatientDetailsViewModel(PatientModel patient, List<IntakeModel> intake)
        {
            Patient = patient;
            Intake = intake;
        }
    }
}
