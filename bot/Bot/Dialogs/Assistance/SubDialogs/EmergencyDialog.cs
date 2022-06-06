using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.MedicalData;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

namespace CoreBot.Dialogs.Assistance.SubDialogs
{
	public class EmergencyDialog : ComponentDialog
	{
        private DataServiceConnection connection;
        private IConfiguration configuration;

        public EmergencyDialog(DataServiceConnection connection, IConfiguration configuration)
            : base(nameof(EmergencyDialog))
        {
            AddDialog(new WaterfallDialog("Emergency", new WaterfallStep[]
            {
                SendAlert
            }));

            this.connection = connection;
            this.configuration = configuration;
        }

        private async Task<DialogTurnResult> SendAlert(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "message", $"Er is hulp vereist bij patient {stepContext.Context.Activity.From}!" }
            };

            string url = $"https://{configuration["WebServiceHostName"]}/alert";
            var data = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Een hulpoproep is verzonden! Een zorgmedewerker zal zo snel mogelijk bevestigen onderweg te zijn!"));
            } else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("De hulpoproep kon niet worden verzonden."));
            }

            //Save the Emergency call to the database using the dataservice
            url = $"https://{configuration["DataServiceHostName"]}/api/PatientEmergency";
            values = new Dictionary<string, string>
            {
                { "patientId", "1" },
                { "date", DateTime.Now.ToString() }
            };
            data = new FormUrlEncodedContent(values);
            response = await client.PostAsync(url, data);

            
            return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
        }
    }
}

