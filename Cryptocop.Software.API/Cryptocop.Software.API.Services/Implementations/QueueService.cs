using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private readonly string _exchangeName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public QueueService(IConfiguration configuration)
        {
            var configSection = configuration.GetSection("RabbitMQ");

            _exchangeName = configSection.GetValue<string>("Exchange");

            IAsyncConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = configSection.GetValue<string>("Host")
            };
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void PublishMessage(string routingKey, object body)
        {
            var json = JsonSerializerHelper.SerializeWithCamelCasing(body); 
            var bytes = Encoding.UTF8.GetBytes(json);

            _channel.ExchangeDeclare(exchange: _exchangeName, type: "topic");
            // _channel.QueueDeclare(queue: "create_order",
            //                      durable: false,
            //                      exclusive: false,
            //                      autoDelete: false,
            //                      arguments: null);

            _channel.BasicPublish(_exchangeName, routingKey, null, body: bytes);
            Console.WriteLine("New Order being processed!");
            //Console.WriteLine(" [x] Sent {0}", json);
        }

        public void Dispose()
        {
            // TODO: Dispose the connection and channel
            GC.SuppressFinalize(this);

            _channel.Dispose();
            _connection.Dispose();
        }
    }
}