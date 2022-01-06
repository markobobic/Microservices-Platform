using CommandService.Consts;
using CommandService.EventsProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService, IDisposable
    {
        private readonly IConfiguration _config;
        private readonly IEventProcessor _eventProcessor;
        private  IConnection _connection;
        private  IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor)
        {
            _config = config;
            _eventProcessor = eventProcessor;
            Task.Run(async () => await InitializeRabbitMQ());
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ModuleHandle, ea) =>
            {
                Debug.WriteLine("--> Event received!");
                var notificationMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                await _eventProcessor.ProcessEvent(notificationMessage);
            };
            _channel.BasicConsume(_queueName, true, consumer);
            return Task.CompletedTask;
        }

        private async Task InitializeRabbitMQ()
        {
            await Task.Run(() =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _config[RabbitMQConf.RabbitMQHost],
                    Port = int.Parse(_config[RabbitMQConf.RabbitMQPort])
                };
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    _channel.ExchangeDeclare(RabbitMQConf.Exchange, ExchangeType.Fanout);
                    _queueName = _channel.QueueDeclare().QueueName;
                    _channel.QueueBind(_queueName, RabbitMQConf.Exchange, "");
                    Debug.WriteLine("---> Listening on the message bus..");
                }
                catch (Exception ex)
                {

                    Debug.WriteLine($"--->Could not connect to the message bus: {ex.InnerException}");
                }
            });
            
        }
        private void RabbitMQConnectionShutDown(object sender, ShutdownEventArgs e) =>
           Debug.WriteLine("---> Connection is shutdown!");

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}
