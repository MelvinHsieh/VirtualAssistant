using System.Threading;
using System.Threading.Tasks;
using CoreBot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class FindScheduleDialog : ComponentDialog
    {
        private DataServiceConnection connection;

        public FindScheduleDialog(DataServiceConnection connection)
            : base(nameof(FindScheduleDialog))
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                Confirm,
                ShowSchedule
            }));

            this.connection = connection;
        }

        private async Task<DialogTurnResult> Confirm(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var messageText = "Zal ik het medicatierooster voor u openen?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowSchedule(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            
            if ((string) stepContext.Result == "ja")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Oké! Eén moment geduld alstublieft, dan pak ik uw gegevens erbij."));

                var intake = await connection.GetRequest($"PatientIntake/Patient/1"); // TODO: Ensure that patient ID is actually equal to currently logged in user (mobile app).
                var activity = new Activity
                {
                    Type = "OPEN_SCHEDULE",
                    Value = intake
                };

                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ik heb je gegevens opgehaald en zal je nu doorverwijzen naar je medicatieoverzicht!"));
                await stepContext.Context.SendActivityAsync(activity);
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ik heb je hulpvraag helaas niet begrepen!"));
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
        }
    }
}
