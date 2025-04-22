using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface INewsTagService
    {
        Task<NewsTagModel.CreateModel.Return> AddNewsTag(NewsTagModel.CreateModel.Request request);
    }
}
