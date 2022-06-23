using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreBot;
using CoreBot.Dialogs.Assistance.SubDialogs;
using CoreBot.Models;
using CoreBot.Utils;
using Domain.Entities.MedicalData;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class FindScheduleDialog : ComponentDialog
    {
        private DataServiceConnection connection;
        private MedicineRecognizer _medicineRecognizer;
        List<PatientIntakeModel>? _intakes;

        public FindScheduleDialog(DataServiceConnection connection, MedicineRecognizer medicineRecognizer, List<PatientIntakeModel> intakes = null)
            : base(nameof(FindScheduleDialog))
        {
            AddDialog(new WaterfallDialog("findSchedule", new WaterfallStep[]
            {
                ShowSchedule,
                RegisterIntake
            }));

            this.connection = connection;
            _medicineRecognizer = medicineRecognizer;
            _intakes = intakes;
        }

        private async Task<DialogTurnResult> ShowSchedule(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var intakesToDo = new List<PatientIntakeModel>();
            var builder = new StringBuilder();

            var result = await connection.GetRequest($"PatientIntake/Patient/1");

            List<PatientIntakeModel>? intakes = JsonConvert.DeserializeObject<List<PatientIntakeModel>>(result);

            _intakes = intakes.Where(intake => intake.IntakeStart < DateTime.Now.TimeOfDay && 
                                        (intake.IntakeEnd <= DateTime.Now.TimeOfDay || intake.IntakeEnd >= DateTime.Now.TimeOfDay))
                                        .ToList();

            if (_intakes != null && _intakes.Count > 0)
            {
                PatientIntakeModel intake = _intakes[_intakes.Count-1];

                    if (intake.Medicine.ImageURL != String.Empty)
                    {
                        var image = new Activity
                        {
                            Type = "image",
                            Value = intake.Medicine.ImageURL
                        };

                        await stepContext.Context.SendActivityAsync(image);
                    }

                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Neem nu " + intake.Amount + " " + intake.Medicine.Color + " " + intake.Medicine.Shape + " " + intake.Medicine.Type + " " + intake.Medicine.Name));           
                builder.Append("Heb je dit medicijn ingenomen?");

                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(builder.ToString(), null, InputHints.ExpectingInput) }, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Je hoeft op dit moment geen medicijnen in te nemen."));
            }

            return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
           
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
                    PatientIntakeModel currentIntake = _intakes[_intakes.Count - 1];
                    IntakeRegistration intakeRegistration = new IntakeRegistration() { PatientIntakeId = currentIntake.Id, TakenOn = DateTime.Now };

                    var response = await connection.PostRequest("IntakeRegistration", JsonConvert.SerializeObject(new { PatientIntakeId = intakeRegistration.PatientIntakeId, Date = intakeRegistration.TakenOn }));
                    
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Uw inname is geregistreerd."));
                        _intakes.Remove(currentIntake);
                    }
                    else
                    {
                        await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Het registreren van de inname is mislukt."));
                    }

                    if(_intakes.Count > 0)
                    {
                        return await stepContext.ReplaceDialogAsync(nameof(FindScheduleDialog), _intakes, cancellationToken);
                    }
                    
                    _intakes = null;

                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Je hebt alle medicijnen voor nu genomen."));
                    return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
                //End Dialog
                case nameof(Intents.Cancel):
                    //Cancel the registration attempt
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("De registratie poging is geannuleerd."));
                    _intakes = null;
                    return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
                //End Dialog
                default:
                    //Not understood
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ik heb je antwoord helaas niet begrepen!"));
                    _intakes = null;
                    return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
            }

        }
    }
}
