// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Models;
using Domain.Entities.MedicalData;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Newtonsoft.Json;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class LUISDoseDialog : ComponentDialog
    {
        public LUISDoseDialog()
            : base(nameof(LUISDoseDialog))
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
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5257"); // TODO: In appsettings zetten, extraheren naar aparte business method en rekening houden met migratie van de dataservice naar docker container ipv lokaal gehost.

            Medicine medicine = JsonConvert.DeserializeObject<List<Medicine>>(await client.GetStringAsync("api/medicine")).FirstOrDefault(m => m.Name == (String) stepContext.Result);

            if (medicine != null)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"De dosering '{medicine.Name}' is {medicine.Dose} {medicine.DoseUnit}."));
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Van dit medicijn is geen dosering bekend."));
            }

            
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
