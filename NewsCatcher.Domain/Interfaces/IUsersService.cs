using NewsCatcher.Models.Models;

namespace NewsCatcher.Domain.Interfaces
{
    public interface IUsersService
    {
        Task<UsersModel.BrowseModel.Return> BrowseUsers(UsersModel.BrowseModel.Request request);
        Task<UsersModel.CreateModel.Return> AddUser(UsersModel.CreateModel.Request request);
        Task<UsersModel.UpdateModel.Return> UpdateUser(UsersModel.UpdateModel.Request request);
        Task<UsersModel.DeleteModel.Return> DeleteUser(UsersModel.DeleteModel.Request request);
    }
}
