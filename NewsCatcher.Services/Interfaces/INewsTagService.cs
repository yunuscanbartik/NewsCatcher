using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsCatcherApi.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface INewsTagService
    {
        Task<NewsTagModel.CreateModel.Return> AddNewsTagAsync(NewsTagModel.CreateModel.Request request);
    }
}
