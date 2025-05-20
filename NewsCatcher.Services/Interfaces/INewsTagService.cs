using NewsCatcher.Models.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface INewsTagService
    {
        Task<NewsTagModel.CreateModel.Return> AddNewsTagAsync(NewsTagModel.CreateModel.Request request);
    }
}
