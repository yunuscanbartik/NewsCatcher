using NewsCatcher.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Interfaces
{
    public interface INewsService
    {
        Task<NewsModel.BrowseModel.Return> GetNewsAsync(NewsModel.BrowseModel.Request request);
        Task<NewsModel.BrowseModel.Return> GetNewsByIdAsync(NewsModel.BrowseModel.Request request);
        Task<NewsModel.CreateModel.Return> AddNewsAsync(NewsModel.CreateModel.Request request);
        Task<NewsModel.UpdateModel.Return> UpdateNewsAsync(NewsModel.UpdateModel.Request request);
        Task<NewsModel.DeleteModel.Return> DeleteGetNewsAsync(NewsModel.DeleteModel.Request request);
    }
}
