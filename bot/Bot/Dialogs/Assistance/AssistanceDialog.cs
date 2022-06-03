using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot;
using CoreBot.Dialogs.Assistance.SubDialogs;
using CoreBot.Utils;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class AssistanceDialog : ComponentDialog
    {
        private MedicineRecognizer _medicineRecognizer;

        public AssistanceDialog(MedicineRecognizer medicineRecognizer, DataServiceConnection connection)
            : base(nameof(AssistanceDialog))
        {
            _medicineRecognizer = medicineRecognizer;

            AddDialog(new FindScheduleDialog(connection));
            AddDialog(new FindMedicineByAttributesDialog(connection));
            AddDialog(new RegisterIntakeDialog(connection, medicineRecognizer));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog("assistanceDialog", new WaterfallStep[]
            {
                OfferHelpAsync,
                ProcessQuestionAsync
            }));

            InitialDialogId = "assistanceDialog";
        }

        private async Task<DialogTurnResult> OfferHelpAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { }, cancellationToken);
        }

        private async Task<DialogTurnResult> ProcessQuestionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_medicineRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ik mis op dit moment de verbinding met de technologie die mij helpt om je te begrijpen, probeer het op een later moment nog een keer!"));
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            else
            {
                var luisResult = await _medicineRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
                var intents = luisResult.Intents.OrderByDescending(i => i.Value.Score);
                var intent = intents.First().Key;

                switch (intent)
                {
                    case nameof(Intents.Medicine_FindSchedule):
                        return await stepContext.BeginDialogAsync(nameof(FindScheduleDialog), null, cancellationToken);
                    case nameof(Intents.Medicine_FindMedicineByAttributes):
                        return await stepContext.BeginDialogAsync(nameof(FindMedicineByAttributesDialog), luisResult.Entities, cancellationToken);
                    case nameof(Intents.Intake_RegisterIntake):
                        return await stepContext.BeginDialogAsync(nameof(RegisterIntakeDialog), luisResult.Entities, cancellationToken);
                    case nameof(Intents.Cancel):
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text("Oké, laat het mij weten als ik in de toekomst nog iets voor u kan betekenen."));
                        return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
                    default:
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text("Mijn excuses, ik heb de hulpvraag niet begrepen."));
                        return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
                }
            }
        }
    }
}
