using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Producer.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Text;

namespace CoreBot.Producer
{
    public class RabbitMQProducer : IMessageProducer
    {
        private IConfiguration _configuration;

        public RabbitMQProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration.GetSection("MessageBus").Value,
                Port = Int32.Parse(_configuration.GetSection("MessageBusPort").Value)
            };

            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("loggings", exclusive: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "loggings", body: body);
        }
    }
}
