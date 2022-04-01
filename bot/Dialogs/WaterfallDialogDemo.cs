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
    public class WaterfallDialogDemo : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userStateAccessor;

        public WaterfallDialogDemo(IStatePropertyAccessor<UserProfile> userStateAccessor)
            : base(nameof(WaterfallDialogDemo))
        {
            _userStateAccessor = userStateAccessor;

            AddDialog(new TextPrompt(nameof(TextPrompt))); // We voegen de text prompt dialog toe aan de component dialog, zodat we deze kunnen gebruiken tijdens hierop volgende dialogs.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                AskNameAsync,
                AskAgeAsync,
                AskResidencyAsync,
                AskConfirmationAsync
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var user = await _userStateAccessor.GetAsync(stepContext.Context, () => new UserProfile());

            var messageText = stepContext.Options?.ToString() ?? $"Wat is je naam?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskAgeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var user = await _userStateAccessor.GetAsync(stepContext.Context, () => new UserProfile());
            user.Name = (string) stepContext.Result;

            var messageText = stepContext.Options?.ToString() ?? $"Wat is je leeftijd?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }
        private async Task<DialogTurnResult> AskResidencyAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var user = await _userStateAccessor.GetAsync(stepContext.Context, () => new UserProfile());
            user.Age = Int32.Parse((string) stepContext.Result);

            var messageText = stepContext.Options?.ToString() ?? $"Wat is je woonplaats?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskConfirmationAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var user = await _userStateAccessor.GetAsync(stepContext.Context, () => new UserProfile());
            user.Residency = (string) stepContext.Result;

            var messageText = "Kloppen de volgende gegevens?" +
                $"\n Naam: {user.Name}" +
                $"\n Leeftijd: {user.Age}" +
                $"\n Woonplaats: {user.Residency}";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        //private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    if (!_luisRecognizer.IsConfigured)
        //    {
        //        await stepContext.Context.SendActivityAsync(
        //            MessageFactory.Text("LUIS is niet geconfigureerd.", inputHint: InputHints.IgnoringInput), cancellationToken);

        //        return await stepContext.NextAsync(null, cancellationToken);
        //    }

        //    var messageText = stepContext.Options?.ToString() ?? $"Hallo! Waar kan ik u vandaag mee ondersteunen?";
        //    var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
        //    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        //}

        //private async Task<DialogTurnResult> FindIntentAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    var luisResult = await _luisRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
        //    var intents = luisResult.Intents.OrderBy(i => i.Value);

        //    switch (luisResult.Intents.First().Key)
        //    {
        //        case nameof(FlightBooking.Intent.findMedicine):
        //            var medicineQuestion = $"Klopt het dat je benieuwd bent welke medicijnen je moet innemen?";
        //            var medicineQuestionMessage = MessageFactory.Text(medicineQuestion, medicineQuestion, InputHints.IgnoringInput);
        //            await stepContext.Context.SendActivityAsync(medicineQuestionMessage, cancellationToken);

        //            break;
        //        case nameof(FlightBooking.Intent.findMedicineDose):
        //            var medicineDoseQuestion = $"Klopt het dat je iets wilt weten over de dosering van een medicijn?";
        //            var medicineDoseQuestionMessage = MessageFactory.Text(medicineDoseQuestion, medicineDoseQuestion, InputHints.IgnoringInput);
        //            await stepContext.Context.SendActivityAsync(medicineDoseQuestionMessage, cancellationToken);

        //            break;
        //        default:
        //            var catchText = $"Mijn exuses, ik heb de vraag niet begrepen.";
        //            var catchMessage = MessageFactory.Text(catchText, catchText, InputHints.IgnoringInput);
        //            await stepContext.Context.SendActivityAsync(catchMessage, cancellationToken);

        //            break;
        //    }

        //    return await stepContext.NextAsync(null, cancellationToken);
        //}
    }
}
