using NewsCatcher.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NewsCatcher.Models.Models.NewsModel;

namespace NewsCatcher.Services.Interfaces
{
    public interface IRssFeedService
    {
        Task<List<NewsModel.BBCModel.Item>> FetchRssItemsAsync(string feedUrl);
    }
}
