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
                ShowSchedule
            }));

            this.connection = connection;
        }

        private async Task<DialogTurnResult> ShowSchedule(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Het medicijnrooster wordt geopend!"));

            var intake = await connection.GetRequest($"PatientIntake/Patient/1"); // TODO: Ensure that patient ID is actually equal to currently logged in user (mobile app).
            var activity = new Activity
            {
                Type = "OPEN_SCHEDULE",
                Value = intake
            };

            await stepContext.Context.SendActivityAsync(activity);
            return await stepContext.EndDialogAsync(null, cancellationToken);

        }
    }
}
