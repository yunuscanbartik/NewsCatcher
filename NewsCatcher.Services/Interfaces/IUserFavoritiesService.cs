using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsCatcherApi.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface IUserFavoritiesService
    {
        Task<UserFavoritiesModel.BrowseModel.Return> GetUserFavoritiesAsync(UserFavoritiesModel.BrowseModel.Request request);
        Task<UserFavoritiesModel.CreateModel.Return> AddUserFavoritiesAsync(UserFavoritiesModel.CreateModel.Request request);
        Task<UserFavoritiesModel.UpdateModel.Return> UpdateUserFavoritiesAsync(UserFavoritiesModel.UpdateModel.Request request);
        Task<UserFavoritiesModel.DeleteModel.Return> DeleteUserFavoritiesAsync(UserFavoritiesModel.DeleteModel.Request request);
    }
}
