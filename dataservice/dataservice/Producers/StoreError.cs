using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Producer.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Text;

namespace dataservice.Producers
{
    public class StoreError : IMessageProducer
    {
        private IConfiguration _configuration;

        private static string EXCHANGE_NAME;
        private static string QUEUE_NAME;
        private static string USERNAME;
        private static string PASSWORD;

        public StoreError(IConfiguration configuration)
        {
            _configuration = configuration;

            EXCHANGE_NAME = configuration.GetSection("MessageBusExchange").Value;
            QUEUE_NAME = configuration.GetSection("MessageBusQueue").Value;
            USERNAME = configuration.GetSection("MessageBusUsername").Value;
            PASSWORD = configuration.GetSection("MessageBusPassword").Value;
        }

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                UserName = USERNAME,
                Password = PASSWORD,
                HostName = _configuration.GetSection("MessageBus").Value,
                Port = Int32.Parse(_configuration.GetSection("MessageBusPort").Value)
            };

            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Direct, true);

            channel.QueueDeclare(QUEUE_NAME, true, false, false, null);
            channel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, QUEUE_NAME);


            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: EXCHANGE_NAME, routingKey: QUEUE_NAME, body: body);

            channel.Close();
            connection.Close();
        }
    }
}
