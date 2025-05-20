using NewsCatcher.Models.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface IUserFavoritiesService
    {
        Task<UserFavoritiesModel.BrowseModel.Return> GetUserFavoritiesByIdAsync(UserFavoritiesModel.BrowseModel.Request request);
        Task<UserFavoritiesModel.CreateModel.Return> AddUserFavoritiesAsync(UserFavoritiesModel.CreateModel.Request request);
        Task<UserFavoritiesModel.UpdateModel.Return> UpdateUserFavoritiesAsync(UserFavoritiesModel.UpdateModel.Request request);
        Task<UserFavoritiesModel.DeleteModel.Return> DeleteUserFavoritiesAsync(UserFavoritiesModel.DeleteModel.Request request);
    }
}
