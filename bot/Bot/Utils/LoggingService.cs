using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Producer.RabbitMQ;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Utils
{
    public class LoggingService : ITranscriptLogger
    {
        private readonly IMessageProducer _messagePublisher;

        public LoggingService(IMessageProducer messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public Task LogActivityAsync(IActivity activity)
        {
            var messageActivity = activity.AsMessageActivity();

            if(messageActivity != null)
            {
                if(messageActivity.From.Name == "User")
                {
                    _messagePublisher.SendMessage(new LoggingModel()
                    {
                        Id = messageActivity.Id,
                        From = "User",
                        Message = messageActivity.Text
                    });
                } else if (messageActivity.From.Name == "Bot")
                {
                    if(messageActivity.ReplyToId != null)
                    {
                        //TODO hier een betere oplossing voor vinden
                        if(messageActivity.Text != "Welkom!" && 
                            messageActivity.Text != "Waar kan ik u vandaag mee helpen?")
                        {
                            _messagePublisher.SendMessage(
                            new LoggingModel() 
                            { 
                                Id = messageActivity.Id,
                                From = "Bot",
                                ReplyToId = messageActivity.ReplyToId,
                                Message = messageActivity.Text 
                            });
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
