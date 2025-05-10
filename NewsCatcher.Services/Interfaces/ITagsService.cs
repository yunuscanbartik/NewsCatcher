using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsCatcherApi.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface ITagsService
    {
        Task<TagsModel.BrowseModel.Return> GetTagsAsync(TagsModel.BrowseModel.Request request);
        Task<TagsModel.CreateModel.Return> AddTagAsync(TagsModel.CreateModel.Request request);
        Task<TagsModel.UpdateModel.Return> UpdateTagAsync(TagsModel.UpdateModel.Request request);
        Task<TagsModel.DeleteModel.Return> DeleteTagAsync(TagsModel.DeleteModel.Request request);
    }
}
