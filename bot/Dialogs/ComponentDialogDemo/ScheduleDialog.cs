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
    public class ScheduleDialog : ComponentDialog
    {
        public ScheduleDialog()
            : base(nameof(ScheduleDialog))
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                HelpAndConfirmAsync
            }));
        }

        private async Task<DialogTurnResult> HelpAndConfirmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Binnen het systeem staat geregistreerd dat je vandaag alleen paracetamol en diclofenac hoeft in te nemen. Bent u hier voldoende mee geholpen?"));
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
