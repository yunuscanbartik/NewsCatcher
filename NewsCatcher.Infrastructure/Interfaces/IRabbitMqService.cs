using NewsCatcher.Infrastructure.Models;

namespace NewsCatcher.Infrastructure.Interfaces
{
    public interface IRabbitMqService
    {
        bool IsConnected { get; }

        Task<RabbitMqModel.Publish.ReturnData> PublishMessage(RabbitMqModel.Publish.Request request);

        Task<RabbitMqModel.Consume.ReturnData> Consume(RabbitMqModel.Consume.Request request);
    }
}
