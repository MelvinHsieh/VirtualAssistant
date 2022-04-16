using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs.Assistance.SubDialogs
{
    public class FindMedicineByAttributesDialog : ComponentDialog
    {
        private DataServiceConnection connection;

        public FindMedicineByAttributesDialog(DataServiceConnection connection)
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                Inform
            }));

            this.connection = connection;
        }

        private async Task<DialogTurnResult> Inform(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var medicines = 0;
            var messageText = $"Het medicijn waar u het over heeft is {medicineName}! Bent u hier voldoende mee geholpen?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }
    }
}
