using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface INewsStatisticsService
    {
        Task<NewsStatisticsModel.BrowseModel.Return> GetNewsStatistics(NewsStatisticsModel.BrowseModel.Request request);
    }
}
