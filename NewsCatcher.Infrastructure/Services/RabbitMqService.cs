using Microsoft.Extensions.Options;
using NewsCatcher.Infrastructure.Interfaces;
using NewsCatcher.Infrastructure.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NewsCatcher.Infrastructure.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private const ushort DefaultConsumerPrefetchCount = 5;
        private const int DefaultNetworkRecoverySeconds = 10;

        private static class QueueDeclarationOptions
        {
            public const bool Durable = true;
            public const bool Exclusive = false;
            public const bool AutoDelete = false;
        }

        private static class ConsumerOptions
        {
            public const bool AutoAcknowledge = false;
            public const uint PrefetchSizeUnlimited = 0;
            public const bool GlobalPrefetch = false;
        }

        private readonly IConnection _rabbitConnection;
        private readonly IModel _channel;
        private readonly ushort _consumerPrefetchCount;

        public RabbitMqService(IOptions<RabbitMqSettingsModel> rabbitMqSettingsOptions)
        {
            var rabbitMqSettings = rabbitMqSettingsOptions.Value;
            if (string.IsNullOrWhiteSpace(rabbitMqSettings.HostName))
            {
                throw new InvalidOperationException("RabbitMQ HostName must be set in configuration.");
            }

            var recoverySeconds = rabbitMqSettings.NetworkRecoveryIntervalSeconds > 0
                ? rabbitMqSettings.NetworkRecoveryIntervalSeconds
                : DefaultNetworkRecoverySeconds;

            _consumerPrefetchCount = rabbitMqSettings.ConsumerPrefetchCount > 0
                ? (ushort)rabbitMqSettings.ConsumerPrefetchCount
                : DefaultConsumerPrefetchCount;

            var factory = new ConnectionFactory
            {
                HostName = rabbitMqSettings.HostName,
                Port = rabbitMqSettings.Port,
                UserName = rabbitMqSettings.UserName,
                Password = rabbitMqSettings.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(recoverySeconds)
            };
            _rabbitConnection = factory.CreateConnection();
            _channel = _rabbitConnection.CreateModel();
        }

        public bool IsConnected => _rabbitConnection.IsOpen && _channel.IsOpen;

        public Task<RabbitMqModel.Publish.ReturnData> PublishMessage(RabbitMqModel.Publish.Request request)
        {
            var body = Encoding.UTF8.GetBytes(request.Message);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.QueueDeclare(
                request.QueueName,
                QueueDeclarationOptions.Durable,
                QueueDeclarationOptions.Exclusive,
                QueueDeclarationOptions.AutoDelete);
            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: request.QueueName,
                basicProperties: properties,
                body: body);

            return Task.FromResult(new RabbitMqModel.Publish.ReturnData
            {
                IsSuccess = true,
                QueueName = request.QueueName,
                Message = "Message published to queue successfully."
            });
        }

        public Task<RabbitMqModel.Consume.ReturnData> Consume(RabbitMqModel.Consume.Request request)
        {
            if (request.MessageHandler is null)
            {
                return Task.FromResult(new RabbitMqModel.Consume.ReturnData
                {
                    IsSuccess = false,
                    QueueName = request.QueueName,
                    Message = "Message handler cannot be null."
                });
            }

            _channel.QueueDeclare(
                request.QueueName,
                QueueDeclarationOptions.Durable,
                QueueDeclarationOptions.Exclusive,
                QueueDeclarationOptions.AutoDelete);

            _channel.BasicQos(ConsumerOptions.PrefetchSizeUnlimited, _consumerPrefetchCount, ConsumerOptions.GlobalPrefetch);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (_, deliveryEventArgs) =>
            {
                var body = deliveryEventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await request.MessageHandler(message);
                _channel.BasicAck(deliveryEventArgs.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(
                request.QueueName,
                ConsumerOptions.AutoAcknowledge,
                consumer);

            return Task.FromResult(new RabbitMqModel.Consume.ReturnData
            {
                IsSuccess = true,
                QueueName = request.QueueName,
                Message = "Consumer started successfully."
            });
        }
    }
}
