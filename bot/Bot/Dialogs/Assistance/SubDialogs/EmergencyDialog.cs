using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.MedicalData;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace CoreBot.Dialogs.Assistance.SubDialogs
{
	public class EmergencyDialog : ComponentDialog
	{
        private DataServiceConnection connection;

        public EmergencyDialog(DataServiceConnection connection)
            : base(nameof(EmergencyDialog))
        {
            AddDialog(new WaterfallDialog("sendAlert", new WaterfallStep[]
            {
                SendAlert
            }));

            this.connection = connection;
        }

        private async Task<DialogTurnResult> SendAlert(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "message", "Er is hulp vereist bij patient ...!" }
            };

            string url = "https://localhost:7153/alert";
            var data = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Een hulpoproep is verzonden!"));
            } else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("De hulpoproep kon niet worden verzonden."));
            }

            //Save the Emergency call to the database using the dataservice
            url = "https://localhost:7257/api/PatientEmergency";
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

