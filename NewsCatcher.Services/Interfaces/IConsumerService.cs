using NewsCatcher.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Interfaces
{
    public interface IConsumerService
    {
        Task<List<NewsModel.CreateModel.ReturnData>> SaveToDatabaseAsync(List<NewsModel.CreateModel.ReturnData> returnDataList);
    }
}
