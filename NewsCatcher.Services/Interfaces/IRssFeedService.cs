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
        Task<List<NewsModel.BBCModel.Item>> GetRssItemsAsync(string feedUrl);
        Task<List<NewsModel.CreateModel.ReturnData>> MapToReturnDataAsync(List<NewsModel.BBCModel.Item> bbcItems);
        Task<List<NewsModel.CreateModel.ReturnData>> SaveToDatabaseAsync(List<NewsModel.CreateModel.ReturnData> returnDataList);
    }
}