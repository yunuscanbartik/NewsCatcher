using NewsCatcherApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Interfaces
{
    public interface INewsStatisticsService
    {
        Task<NewsStatisticsModel.BrowseModel.Return> GetNewsStatisticsByIdAsync(NewsStatisticsModel.BrowseModel.Request request);
    }
}
