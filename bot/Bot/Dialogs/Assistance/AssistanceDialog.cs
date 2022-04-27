using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot;
using CoreBot.Dialogs.Assistance.SubDialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
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
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                OfferHelpAsync,
                ProcessQuestionAsync
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> OfferHelpAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var messageText = "Waar kan ik u vandaag mee helpen?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ProcessQuestionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_medicineRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ik mis op dit moment de verbinding met de technologie die mij helpt om je te begrijpen, probeer het op een later moment nog een keer!."));
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
                    default:
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text("Mijn excuses, ik heb de hulpvraag niet begrepen."));
                        return await stepContext.EndDialogAsync(null, cancellationToken);
                }
            }
        }
    }

    public enum Intents
    {
        Medicine_ConfirmMedicine,
        Medicine_FindDose,
        Medicine_FindMedicineByAttributes,
        Medicine_FindSchedule,
        Medicine_GetMedicineInfo
    }
}
