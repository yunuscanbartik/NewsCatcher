using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface IGenericRssFeedService
    {
        Task<List<NewsModel.CreateModel.ReturnData>> FetchAsync(string feedUrl, string sourceName, CancellationToken cancellationToken = default);
    }
}
