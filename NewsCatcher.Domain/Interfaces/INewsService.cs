using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface INewsService
    {
        Task<NewsModel.BrowseModel.Return> GetNews(NewsModel.BrowseModel.Request request);
        Task<NewsModel.CreateModel.Return> AddNews(NewsModel.CreateModel.Request request);
        Task<NewsModel.UpdateModel.Return> UpdateNews(NewsModel.UpdateModel.Request request);
        Task<NewsModel.DeleteModel.Return> DeleteNews(NewsModel.DeleteModel.Request request);
        Task<List<NewsModel.CreateModel.ReturnData>> SaveToDatabase(List<NewsModel.CreateModel.ReturnData> returnDataList);
    }
}
