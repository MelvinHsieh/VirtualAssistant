namespace web.Models.CreateModels
{
    public class CareWorkerCreateModel
    {
        public RegisterModel AccountData { get; set; } = new RegisterModel();
        public CareWorkerModel CareWorkerData { get; set; } = new CareWorkerModel(); 
    }
}
