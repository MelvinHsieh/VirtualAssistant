// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using CoreBot.Models;
using CoreBot.Producer;
using CoreBot.Utils;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.TraceExtensions;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Microsoft.BotBuilderSamples
{
    public class AdapterWithErrorHandler : CloudAdapter
    {
        public AdapterWithErrorHandler(TranscriptLoggerMiddleware loggingMiddleware, BotFrameworkAuthentication auth, StoreError errorLogger, ConversationState conversationState = default)
            : base(auth)
        {
            Use(loggingMiddleware);

            OnTurnError = async (turnContext, exception) =>
            {
                // Log any leaked exception from the application.
                // NOTE: In production environment, you should consider logging this to
                // Azure Application Insights. Visit https://aka.ms/bottelemetry to see how
                // to add telemetry capture to your bot.
                errorLogger.SendMessage(new ErrorModel() { ErrorMessage = $"[OnTurnError] unhandled error : {exception.Message}" });

                // Send a message to the user
                var errorMessageText = "Ik weet helaas even niet meer wat ik moet doen.";
                var errorMessage = MessageFactory.Text(errorMessageText, errorMessageText, InputHints.IgnoringInput);
                await turnContext.SendActivityAsync(errorMessage);

                errorMessageText = "Neem alsjeblieft contact op met de administrator.";
                errorMessage = MessageFactory.Text(errorMessageText, errorMessageText, InputHints.IgnoringInput);
                await turnContext.SendActivityAsync(errorMessage);

                if (conversationState != null)
                {
                    try
                    {
                        // Delete the conversationState for the current conversation to prevent the
                        // bot from getting stuck in a error-loop caused by being in a bad state.
                        // ConversationState should be thought of as similar to "cookie-state" in a Web pages.
                        await conversationState.DeleteAsync(turnContext);
                    }
                    catch (Exception e)
                    {
                        errorLogger.SendMessage(new ErrorModel() { ErrorMessage = $"Exception caught on attempting to Delete ConversationState : {e.Message}" });
                    }
                }

                // Send a trace activity, which will be displayed in the Bot Framework Emulator
                await turnContext.TraceActivityAsync("OnTurnError Trace", exception.Message, "https://www.botframework.com/schemas/error", "TurnError");
            };
        }
    }
}
