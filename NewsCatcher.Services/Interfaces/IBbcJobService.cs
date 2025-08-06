using NewsCatcher.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Interfaces
{
    public interface IBbcJobService
    {
        Task<List<NewsModel.BBCModel.Item>> GetRssItemsAsync(string feedUrl);
        Task<List<NewsModel.CreateModel.ReturnData>> MapToReturnDataAsync(List<NewsModel.BBCModel.Item> bbcItems);
        //Task<List<NewsModel.CreateModel.ReturnData>> SendQueue(List<NewsModel.CreateModel.ReturnData> returnDataList);
    }
}
