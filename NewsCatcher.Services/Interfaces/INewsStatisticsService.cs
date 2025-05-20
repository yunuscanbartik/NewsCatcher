using NewsCatcher.Models.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface INewsStatisticsService
    {
        Task<NewsStatisticsModel.BrowseModel.Return> GetNewsStatisticsByIdAsync(NewsStatisticsModel.BrowseModel.Request request);
    }
}
