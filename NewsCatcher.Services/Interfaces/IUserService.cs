using NewsCatcher.Models.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface IUserService
    {
        Task<UsersModel.LoginModel.Return> UserLoginAsync(UsersModel.LoginModel.Request request);
    }
}
