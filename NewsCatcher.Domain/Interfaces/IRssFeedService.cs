using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface IRssFeedService
    {
        Task<List<NewsModel.BBCModel.Item>> GetRssItems(string feedUrl);
        Task<List<NewsModel.CreateModel.ReturnData>> MapToReturnData(List<NewsModel.BBCModel.Item> bbcItems);
        Task<List<NewsModel.CreateModel.ReturnData>> SaveToDatabase(List<NewsModel.CreateModel.ReturnData> returnDataList);
    }
}
