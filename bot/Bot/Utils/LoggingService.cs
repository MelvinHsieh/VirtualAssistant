using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Producer.RabbitMQ;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Utils
{
    public class LoggingService : ITranscriptLogger
    {
        private readonly IMessageProducer _messagePublisher;
        protected readonly IConfiguration _configuration;

        public LoggingService(IMessageProducer messagePublisher, IConfiguration configuration)
        {
            _messagePublisher = messagePublisher;
            _configuration = configuration;
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
                        if(messageActivity.Text != _configuration.GetSection("WelcomeMessage").Value && 
                            messageActivity.Text != _configuration.GetSection("HelpMessage").Value)
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
