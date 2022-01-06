using Microsoft.Extensions.Configuration;
using PlatformService.DTOs;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private const string _exchange = "trigger";
        private const string _rabbitMQHost = "RabbitMQHost";
        private const string _rabbitMQPort = "RabbitMQPort";
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            var factory = new ConnectionFactory()
            {
                HostName = _config[_rabbitMQHost],
                Port = int.Parse(_config[_rabbitMQPort])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQConnectionShutDown;
                Console.WriteLine("---> Connected to message bus.");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"--->Could not connect to the message bus: {ex.InnerException}");
            }
        }

        private void RabbitMQConnectionShutDown(object sender, ShutdownEventArgs e) =>
            Console.WriteLine("---> Connection is shutdown!");

        public async Task PublishNewPlatformAsync(PlatformPublishedDTO platformPublishedDTO)
        {
            var message = JsonSerializer.Serialize(platformPublishedDTO);
            if (_connection.IsOpen)
            {
                Console.WriteLine("---> RabbitMQ connection open! Sending message..");
                await SendMessageAsync(message);
                Console.WriteLine($"--> We have sent {message}");
                return;
            }
            Console.WriteLine("---> RabbitMQ connection is closed, not sending!");
        }
        private async Task SendMessageAsync(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            await Task.Run(() =>
            {
                _channel.BasicPublish(_exchange, "", null, body);
            });
        }

        public void Dispose()
        {
            Console.WriteLine("Message bus disposed!");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
