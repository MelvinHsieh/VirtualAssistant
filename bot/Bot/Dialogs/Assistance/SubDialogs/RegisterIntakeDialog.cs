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
    public class RegisterIntakeDialog : ComponentDialog
    {
        private DataServiceConnection connection;

        public RegisterIntakeDialog(DataServiceConnection connection)
            : base(nameof(RegisterIntakeDialog))
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                Confirm,
                RegisterIntake
            }));

            this.connection = connection;
        }

        private async Task<DialogTurnResult> Confirm(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Set up register object
            IntakeRegistration intakeRegistration = new IntakeRegistration();
            var intakes = JsonConvert.DeserializeObject<List<PatientIntake>>(await connection.GetRequest($"patientIntake/patient/{1}")); //TODO get id from android app
            var matchingIntakes = new List<PatientIntake>();

            string medicineName = ((JObject)stepContext.Options)["Intake"]?["Medicine"]?.Children().First().First().Value<string>();
            if(medicineName != null) //if medicineName 
            {
                matchingIntakes = intakes
                   .Where(i => string.IsNullOrEmpty(medicineName) ? true : i.Medicine.Name.ToLower() == medicineName.ToLower())
                   .ToList();

            }
            else //else if attributes
            {
                string color = ((JObject)stepContext.Options)["Color"]?.Children().First().First().Value<string>();
                string type = ((JObject)stepContext.Options)["Type"]?.Children().First().First().Value<string>();
                var shape = ((JObject)stepContext.Options)["Shape"]?.Children().First().First().Value<string>();

                matchingIntakes = intakes
                    .Where(i => string.IsNullOrEmpty(color) ? true : i.Medicine.Color.ToLower() == color.ToLower())
                    .Where(i => string.IsNullOrEmpty(type) ? true : i.Medicine.Type.ToLower() == type.ToLower())
                    .Where(i => string.IsNullOrEmpty(shape) ? true : i.Medicine.Shape.ToLower() == shape.ToLower())
                    .ToList();
            }

            //ExtractDate 
            DateTime date;
            if(DateTime.TryParse(((JObject)stepContext.Options)["datetimeV2"]?.Children().First().First().Value<string>(), out date)) //If date
            {
                intakeRegistration.TakenOn = date;
                
            } else //what if no date?? fail action? use DateTime now?
            {
                intakeRegistration.TakenOn = DateTime.Now;
            }

            var timeMatchedIntakes = matchingIntakes 
                    .Where(i => i.IntakeStart.TimeOfDay < intakeRegistration.TakenOn.TimeOfDay && i.IntakeEnd.TimeOfDay > intakeRegistration.TakenOn.TimeOfDay)
                    .ToList();

            if(timeMatchedIntakes.Count > 0)
            {
                matchingIntakes = timeMatchedIntakes;
            }

            //ask for confirmation
            var builder = new StringBuilder();

            if (matchingIntakes.Count > 1)
            {
                builder.Append("Er staan binnen het systeem meerdere medicijnen inname momenten aan u geregistreerd die aan die beschrijving voldoen, namelijk ");

                for (int i = 0; i < matchingIntakes.Count - 1; i++)
                {
                    builder.Append($"{matchingIntakes[i].Medicine.Name} tussen {matchingIntakes[i].IntakeStart.TimeOfDay} en {matchingIntakes[i].IntakeEnd.TimeOfDay}, ");
                }

                builder.Append($" en {matchingIntakes.Last().Medicine.Name}. Probeer het opnieuw"); //TODO Rephrase

                await stepContext.Context.SendActivityAsync(MessageFactory.Text(builder.ToString()));
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            else if (matchingIntakes.Count == 1)
            {
                intakeRegistration.PatientIntakeId = matchingIntakes.First().Id;
                intakeRegistration.PatientIntake = matchingIntakes.First();
                builder.Append($"Klopt het dat u {intakeRegistration.PatientIntake.Medicine.Name} heeft ingenomen rond {intakeRegistration.TakenOn}?");
                stepContext.Values.Add("intake", intakeRegistration);

                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(builder.ToString(), null, InputHints.ExpectingInput) }, cancellationToken);
            }
            else
            {
                builder.Append("Mijn excuses! Binnen het systeem staan geen medicijnen aan u gekoppeld die aan die eisen voldoen. Zal ik u doorverbinden met een medewerker?");
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(builder.ToString()));
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> RegisterIntake(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Register
            if ((string)stepContext.Result == "Ja")
            {
                IntakeRegistration intakeRegistration = (IntakeRegistration)stepContext.Values["intake"];
                


                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Uw inname voor."));
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ik heb je hulpvraag helaas niet begrepen!"));
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
        }
    }
}
