using Domain.Entities.MedicalData;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Utils;
using Microsoft.BotBuilderSamples;

namespace CoreBot.Dialogs.Assistance.SubDialogs
{
    public class RegisterIntakeDialog : ComponentDialog
    {
        private DataServiceConnection connection;
        private MedicineRecognizer _medicineRecognizer;

        public RegisterIntakeDialog(DataServiceConnection connection, MedicineRecognizer medicineRecognizer)
            : base(nameof(RegisterIntakeDialog))
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                Confirm,
                RegisterIntake
            }));

            this.connection = connection;
            _medicineRecognizer = medicineRecognizer;
        }

        private async Task<DialogTurnResult> Confirm(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //ask for confirmation
            var culture = CultureInfo.GetCultureInfo("nl-NL");
            CultureInfo.CurrentCulture = culture;
            var builder = new StringBuilder();

            //Set up register object
            IntakeRegistration intakeRegistration = new IntakeRegistration();
            var intakes = JsonConvert.DeserializeObject<List<PatientIntake>>(await connection.GetRequest($"patientIntake/patient/{1}")); //TODO get id from android app
            var matchingIntakes = new List<PatientIntake>();

            var medicine = ((JObject)stepContext.Options)["Medicine"];
            string color = ((JObject)stepContext.Options)["Color"]?.Children().First().First().Value<string>();
            string type = ((JObject)stepContext.Options)["Type"]?.Children().First().First().Value<string>();
            var shape = ((JObject)stepContext.Options)["Shape"]?.Children().First().First().Value<string>();
            if (medicine != null) {
                string medicineName = medicine.First().First().First()[0].Value<string>();
                if (medicineName != null) //if medicineName 
                {
                    matchingIntakes = intakes
                       .Where(i => string.IsNullOrEmpty(medicineName) ? true : i.Medicine.Name.ToLower() == medicineName.ToLower())
                       .ToList();

                }
            }
            else if(color != null || type != null || shape != null) //else if attributes
            {
                matchingIntakes = intakes
                    .Where(i => string.IsNullOrEmpty(color) ? true : i.Medicine.Color.ToLower() == color.ToLower())
                    .Where(i => string.IsNullOrEmpty(type) ? true : i.Medicine.Type.ToLower() == type.ToLower())
                    .Where(i => string.IsNullOrEmpty(shape) ? true : i.Medicine.Shape.ToLower() == shape.ToLower())
                    .ToList();
            } else
            {
                builder.Append("Mijn excuses! Het systeem heeft geen medicijn kunnen herkennen. Bij problemen kunt u contact opnemen met een medewerker");
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(builder.ToString()));
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }

            if(((JObject)stepContext.Options)["datetime"] != null) //Extract Date
            {
                if(((JObject)stepContext.Options)["datetime"].HasValues) { 
                    string firstOption = ((JObject)stepContext.Options)["datetime"].Children().First().First().First[0].Value<string>();

                    intakeRegistration.TakenOn = TimexToDateTimeConverter.TryTimexToDateTime(firstOption);
                } else
                {
                    intakeRegistration.TakenOn = DateTime.Now;
                }
            } else
            {
                intakeRegistration.TakenOn = DateTime.Now;
            }

            var timeMatchedIntakes = matchingIntakes 
                    .Where(i => i.IntakeStart.TimeOfDay <= intakeRegistration.TakenOn.TimeOfDay && i.IntakeEnd.TimeOfDay >= intakeRegistration.TakenOn.TimeOfDay)
                    .ToList();

            if(timeMatchedIntakes.Count > 0)
            {
                matchingIntakes = timeMatchedIntakes;
            }

            if (matchingIntakes.Count > 1)
            {
                builder.Append("Er staan binnen het systeem meerdere medicijnen inname momenten aan u geregistreerd die aan die beschrijving voldoen, namelijk ");

                for (int i = 0; i < matchingIntakes.Count - 1; i++)
                {
                    builder.Append($"{matchingIntakes[i].Medicine.Name} tussen {matchingIntakes[i].IntakeStart.TimeOfDay} en {matchingIntakes[i].IntakeEnd.TimeOfDay}, \n");
                }

                builder.Append($" en {matchingIntakes.Last().Medicine.Name}. Probeer het opnieuw"); //TODO Rephrase

                await stepContext.Context.SendActivityAsync(MessageFactory.Text(builder.ToString()));
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
            else if (matchingIntakes.Count == 1)
            {
                intakeRegistration.PatientIntakeId = matchingIntakes.First().Id;
                intakeRegistration.PatientIntake = matchingIntakes.First();
                builder.Append($"Klopt het dat u {intakeRegistration.PatientIntake.Medicine.Name} heeft ingenomen op { intakeRegistration.TakenOn.ToString("m")} rond {intakeRegistration.TakenOn.ToString("t")}?");
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
            var luisResult = await _medicineRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
            var intents = luisResult.Intents.OrderByDescending(i => i.Value.Score);
            var intent = intents.First().Key;

            switch (intent)
            {
                case nameof(Intents.Confirm):
                    //Start the registration
                    IntakeRegistration intakeRegistration = (IntakeRegistration)stepContext.Values["intake"];

                    var response = await connection.PostRequest("IntakeRegistration", JsonConvert.SerializeObject(new { PatientIntakeId = intakeRegistration.PatientIntakeId, Date = intakeRegistration.TakenOn }));
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Uw inname is geregistreerd"));
                    }
                    else
                    {
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Het registreren van de inname is mislukt"));
                    }

                    return await stepContext.EndDialogAsync(null, cancellationToken);
                    //End Dialog
                case nameof(Intents.Cancel):
                    //Cancel the registration attempt
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("De registratie poging is geanuleerd"));
                    return await stepContext.EndDialogAsync(null, cancellationToken);
                    //End Dialog
                default:
                    //Not understood
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ik heb je antwoord helaas niet begrepen!"));
                    return await stepContext.EndDialogAsync(null, cancellationToken);
            }

        }
    }
}
