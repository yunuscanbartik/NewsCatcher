using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface IUserFavoritiesService
    {
        Task<UserFavoritiesModel.BrowseModel.Return> GetUserFavorities(UserFavoritiesModel.BrowseModel.Request request);
        Task<UserFavoritiesModel.CreateModel.Return> AddUserFavorities(UserFavoritiesModel.CreateModel.Request request);
        Task<UserFavoritiesModel.UpdateModel.Return> UpdateUserFavorities(UserFavoritiesModel.UpdateModel.Request request);
        Task<UserFavoritiesModel.DeleteModel.Return> DeleteUserFavorities(UserFavoritiesModel.DeleteModel.Request request);
    }
}
