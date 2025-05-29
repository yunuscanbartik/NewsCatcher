using NewsCatcher.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UsersModel.BrowseModel.Return> BrowseUsersByIdAsync(UsersModel.BrowseModel.Request request);
        Task<UsersModel.BrowseModel.Return> BrowseUsersAsync(UsersModel.BrowseModel.Request request);
        Task<UsersModel.CreateModel.Return> AddUserAsync(UsersModel.CreateModel.Request request);
        Task<UsersModel.UpdateModel.Return> UpdateUserAsync(UsersModel.UpdateModel.Request request);
        Task<UsersModel.DeleteModel.Return> DeleteUserAsync(UsersModel.DeleteModel.Request request);
    }
}
