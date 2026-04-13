using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface ICategoriesService
    {
        Task<CategoriesModel.BrowseModel.Return> GetCategories(CategoriesModel.BrowseModel.Request request);
        Task<CategoriesModel.CreateModel.Return> AddCategory(CategoriesModel.CreateModel.Request request);
        Task<CategoriesModel.UpdateModel.Return> UpdateCategory(CategoriesModel.UpdateModel.Request request);
        Task<CategoriesModel.DeleteModel.Return> DeleteCategory(CategoriesModel.DeleteModel.Request request);
    }
}
