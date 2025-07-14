using Microsoft.Extensions.Options;
using NewsCatcher.RabbitMQ.Interfaces;
using NewsCatcher.RabbitMQ.Models;
using NewsCatcher.Services.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.RabbitMQ.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly RabbitMqSettingsModel _settings;
        private readonly IDatabaseContext _dbContext;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public RabbitMqService(RabbitMqSettingsModel settings, IDatabaseContext dbContext, IOptions<RabbitMqSettingsModel> options)
        {
            _dbContext = dbContext ;        
            _settings = options.Value;
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                throw new Exception("RabbitMQ bağlantısı kurulamadı: " + ex.Message);
            }
        }
        public void PublishMessage(string message, string queueName)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);
                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                _channel.QueueDeclare(
                    queueName,
                    true,
                    false,
                    false);
                _channel.BasicPublish(
                    "",
                    queueName,
                    properties,
                    body
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Mesaj kuyruğa gönderilemedi: " + ex.Message);
            }
        }
        public void Consume(string queueName, Func<string, Task> messageHandler)
        {
            _channel.QueueDeclare(
                queueName,
                true,
                false,
                false);
            _channel.BasicQos(0, 5, false);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    await messageHandler(message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                    throw new Exception($"Mesaj işlenirken hata oluştu: {ex.Message}");
                }
            };

            _channel.BasicConsume(
                queueName,
                false,
                consumer);
        }
    }
}
