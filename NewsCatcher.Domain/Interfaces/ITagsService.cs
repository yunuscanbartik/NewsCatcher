using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface ITagsService
    {
        Task<TagsModel.BrowseModel.Return> GetTags(TagsModel.BrowseModel.Request request);
        Task<TagsModel.CreateModel.Return> AddTag(TagsModel.CreateModel.Request request);
        Task<TagsModel.UpdateModel.Return> UpdateTag(TagsModel.UpdateModel.Request request);
        Task<TagsModel.DeleteModel.Return> DeleteTag(TagsModel.DeleteModel.Request request);
    }
}
