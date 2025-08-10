using NewsCatcher.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Interfaces
{
    public interface ICnnJobService
    {
        Task<List<NewsModel.CNNModel.Item>> GetRssItemsAsync(string feedUrl);
        Task<List<NewsModel.CreateModel.ReturnData>> MapToReturnDataAsync(List<NewsModel.CNNModel.Item> bbcItems);
        Task SendToQueueAsync(List<NewsModel.CreateModel.ReturnData> returnDataList, string queueName);
    }
}
