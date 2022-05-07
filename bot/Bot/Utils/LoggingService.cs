using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Utils
{
    public class LoggingService : ITranscriptLogger
    {
        public Task LogActivityAsync(IActivity activity)
        {
            var messageActivity = activity.AsMessageActivity();

            if(messageActivity != null)
            {
                if(messageActivity.From.Name == "User")
                {
                    new LoggingModel() { Id = messageActivity.Id, Message = messageActivity.Text };
                } else if (messageActivity.From.Name == "Bot")
                {
                    if(messageActivity.ReplyToId != null)
                    {

                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
