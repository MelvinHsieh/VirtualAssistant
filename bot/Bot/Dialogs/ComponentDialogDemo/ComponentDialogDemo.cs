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
    public class ComponentDialogDemo : ComponentDialog
    {
        public ComponentDialogDemo()
            : base(nameof(ComponentDialogDemo))
        {
            AddDialog(new ScheduleDialog());
            AddDialog(new DoseDialog());
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
            var messageText = "Kan ik je helpen met je rooster of met je dosering?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ProcessQuestionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            switch ((string) stepContext.Result)
            {
                case "rooster":
                    return await stepContext.BeginDialogAsync(nameof(ScheduleDialog), null, cancellationToken);
                case "dosering":
                    return await stepContext.BeginDialogAsync(nameof(DoseDialog), null, cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Mijn excuses, ik heb de hulpvraag niet begrepen."));
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
