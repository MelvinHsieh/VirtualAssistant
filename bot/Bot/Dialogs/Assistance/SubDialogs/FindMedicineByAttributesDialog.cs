using Domain.Entities.MedicalData;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs.Assistance.SubDialogs
{
    public class FindMedicineByAttributesDialog : ComponentDialog
    {
        private DataServiceConnection connection;

        public FindMedicineByAttributesDialog(DataServiceConnection connection)
            : base(nameof(FindMedicineByAttributesDialog))
        {
            AddDialog(new WaterfallDialog("findMedicineByAttributes", new WaterfallStep[]
            {
                Inform,
                Process
            }));

            this.connection = connection;
        }

        private async Task<DialogTurnResult> Inform(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var medicines = JsonConvert.DeserializeObject<List<Medicine>>(await connection.GetRequest("medicine"));
            string color = ((JObject)stepContext.Options)["Color"]?.Children().First().First().Value<string>();
            string type = ((JObject)stepContext.Options)["Type"]?.Children().First().First().Value<string>();
            var shape = ((JObject)stepContext.Options)["Shape"]?.Children().First().First().Value<string>();

            var matchingMedicines = medicines
                .Where(m => String.IsNullOrEmpty(color) ? true : m.Color.ToLower() == color.ToLower())
                .Where(m => String.IsNullOrEmpty(type) ? true : m.Type.ToLower() == type.ToLower())
                .Where(m => String.IsNullOrEmpty(shape) ? true : m.Shape.ToLower() == shape.ToLower())
                .ToList();

            var builder = new StringBuilder();

            if (matchingMedicines.Count > 1)
            {
                builder.Append("Er staan binnen het systeem meerdere medicijnen geregistreerd die aan die beschrijving voldoen, namelijk ");

                for (int i = 0; i < matchingMedicines.Count - 1; i++)
                {
                    builder.Append($"{matchingMedicines[i].Name}, ");
                }

                builder.Append($" en {matchingMedicines.Last().Name}. Zal ik kijken welke medicijnen er specifiek aan u gekoppeld zijn?");

                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(builder.ToString(), null, InputHints.ExpectingInput) }, cancellationToken);
            } else if (matchingMedicines.Count == 1)
            {
                builder.Append($"Het medicijn waar u het over heeft is {matchingMedicines.First().Name}! Bent u hier voldoende mee geholpen?");

                if(matchingMedicines[0].ImageURL != String.Empty)
                {
                    var activity = new Activity
                    {
                        Type = "image",
                        Value = matchingMedicines[0].ImageURL
                    };

                    await stepContext.Context.SendActivityAsync(activity);
                }
             
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(builder.ToString(), null, InputHints.ExpectingInput) }, cancellationToken);
            } else
            {
                builder.Append("Mijn excuses! Binnen het systeem staan geen medicijnen geregistreerd die aan die eisen voldoen. Zal ik u doorverbinden met een medewerker?");
                
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(builder.ToString(), null, InputHints.ExpectingInput) }, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> Process(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Heeft u verder nog hulpvragen?"));
            return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
        }
    }
}
