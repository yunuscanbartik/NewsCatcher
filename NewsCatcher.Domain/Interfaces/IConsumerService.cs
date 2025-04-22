using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface IConsumerService
    {
        Task<List<NewsModel.CreateModel.ReturnData>> SaveToDatabase(List<NewsModel.CreateModel.ReturnData> returnDataList);
    }
}
