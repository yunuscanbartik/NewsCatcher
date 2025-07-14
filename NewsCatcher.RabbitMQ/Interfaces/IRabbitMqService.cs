using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.RabbitMQ.Interfaces
{
    public interface IRabbitMqService
    {
        void PublishMessage(string message, string queueName);
        void Consume(string queueName, Func<string, Task> messageHandler);
    }
}
