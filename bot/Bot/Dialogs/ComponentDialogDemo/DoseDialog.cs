// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
    public class DoseDialog : ComponentDialog
    {
        public DoseDialog()
            : base(nameof(DoseDialog))
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                AskMedicine,
                Confirm
            }));
        }

        private async Task<DialogTurnResult> AskMedicine(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var messageText = "Van welk medicijn wilt u uw dosering weten?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> Confirm(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"U zou vandaag twéé keer {stepContext.Result} moeten innemen. Eén keer om 9 uur 's ochtends, en één keer om 4 uur 's middags."));
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
