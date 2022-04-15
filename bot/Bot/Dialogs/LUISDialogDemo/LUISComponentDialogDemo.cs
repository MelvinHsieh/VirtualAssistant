using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class LUISComponentDialogDemo : ComponentDialog
    {
        private MedicineRecognizer _medicineRecognizer;

        public LUISComponentDialogDemo(MedicineRecognizer medicineRecognizer)
            : base(nameof(LUISComponentDialogDemo))
        {
            _medicineRecognizer = medicineRecognizer;

            AddDialog(new LUISScheduleDialog());
            AddDialog(new LUISDoseDialog());
            AddDialog(new TextPrompt(nameof(TextPrompt))); // We voegen de text prompt dialog toe aan de component dialog, zodat we deze kunnen gebruiken tijdens hierop volgende dialogs.
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
            var luisResult = await _medicineRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
            var intents = luisResult.Intents.OrderByDescending(i => i.Value.Score);
            var intent = intents.First().Key;

            switch (intent)
            {
                case nameof(Intents.findSchedule):
                    return await stepContext.BeginDialogAsync(nameof(LUISScheduleDialog), null, cancellationToken);
                case nameof(Intents.findMedicineDose):
                    return await stepContext.BeginDialogAsync(nameof(LUISDoseDialog), null, cancellationToken);
                default:
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Mijn excuses, ik heb de hulpvraag niet begrepen."));
                    return await stepContext.EndDialogAsync(null, cancellationToken);
            }
        }
    }

    public enum Intents
    {
        findMedicineDose,
        findSchedule
    }
}
