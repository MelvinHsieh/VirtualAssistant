using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.MedicalData;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + configuration["AdminToken"]);
            httpClient.BaseAddress = new Uri($"https://{configuration["WebServiceHostName"]}");

            var content = new
            {
                message = "Een patiënt heeft een noodoproep geplaatst! Klik op dit bericht om naar het dossier van de patiënt te gaan!",
                URI = $"/patient/details/{stepContext.Context.Activity.From.Id}" // TODO: Make connection between android app and bot (combine with other subdialogs).
            };

            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("alert", httpContent);

            if (response.IsSuccessStatusCode)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Een hulpoproep is verzonden! Een zorgmedewerker zal zo snel mogelijk bevestigen onderweg te zijn!"));
            } else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("De hulpoproep kon niet worden verzonden."));
            }

            await LogEmergencyCall(stepContext.Context.Activity.From.Id);

            return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
        }

        private async Task LogEmergencyCall(string patientID)
        {
            var data = new
            {
                patiendId = patientID,
                date = DateTime.Now.ToString()
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + configuration["AdminToken"]);
            httpClient.BaseAddress = new Uri($"https://{configuration["DataServiceHostName"]}");
            await httpClient.PostAsync("api/PatientEmergency", content);

        }
    }
}

