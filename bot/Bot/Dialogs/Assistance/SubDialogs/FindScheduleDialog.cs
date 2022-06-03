using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot;
using CoreBot.Dialogs.Assistance.SubDialogs;
using CoreBot.Models;
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
        List<PatientIntakeModel>? _intakes;

        public FindScheduleDialog(DataServiceConnection connection, List<PatientIntakeModel> intakes = null)
            : base(nameof(FindScheduleDialog))
        {
            AddDialog(new WaterfallDialog("findSchedule", new WaterfallStep[]
            {
                ShowSchedule,
                ConfirmIntake
            }));

            this.connection = connection;
            _intakes = intakes;
        }

        private async Task<DialogTurnResult> ShowSchedule(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Het medicijnrooster wordt geopend!"));

            var intakesToDo = new List<PatientIntakeModel>();

            if (_intakes == null)
            {
                var result = await connection.GetRequest($"PatientIntake/Patient/1"); // TODO: Ensure that patient ID is actually equal to currently logged in user (mobile app).

                List<PatientIntakeModel>? intakes = JsonConvert.DeserializeObject<List<PatientIntakeModel>>(result);

                _intakes = intakes.Where(intake => intake.IntakeStart < DateTime.Now.TimeOfDay && 
                                            (intake.IntakeEnd <= DateTime.Now.TimeOfDay || intake.IntakeEnd >= DateTime.Now.TimeOfDay))
                                            .ToList();
            }

            if (_intakes != null)
            {
                foreach(PatientIntakeModel intake in _intakes)
                {
                    var activity = new Activity
                    {
                        Type = "OPEN_SCHEDULE",
                        Value = intake
                    };

                    await stepContext.Context.SendActivityAsync(activity);

                    if (intake.Medicine.ImageURL != String.Empty)
                    {
                        var image = new Activity
                        {
                            Type = "image",
                            Value = intake.Medicine.ImageURL
                        };

                        await stepContext.Context.SendActivityAsync(image);
                    }

                    var test = await stepContext.NextAsync();


                    /*var promptOptions = new PromptOptions
                    {
                        Prompt = MessageFactory.Text("Heb je dit medicijn ingenomen?"),
                        RetryPrompt = MessageFactory.Text("Neem alsjeblieft dit medicijn in."),
                        Choices = new List<Choice> { new Choice("Ja"), new Choice("Jazeker"), new Choice("Ik heb ze ingenomen"), new Choice("Ze zijn ingenomen"), },
                    };

                    var intakeConfirm = await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);*/

                    
                    /*var response = await connection.PostRequest("IntakeRegistration", JsonConvert.SerializeObject(new { PatientIntakeId = intakeRegistration.PatientIntakeId, Date = intakeRegistration.TakenOn }));*/
                }
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Er staan voor vandaag geen medicijnen meer die je hoeft te slikken!"));
            }

            return await stepContext.BeginDialogAsync("assistanceDialog", null, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmIntake(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Heb je dit medicijn ingenomen?"),
                RetryPrompt = MessageFactory.Text("Neem alsjeblieft dit medicijn in."),
                Choices = new List<Choice> { new Choice("Ja"), new Choice("Jazeker"), new Choice("Ik heb ze ingenomen"), new Choice("Ze zijn ingenomen"), },
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

    }
}
