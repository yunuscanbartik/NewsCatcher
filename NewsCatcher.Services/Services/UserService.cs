using NewsCatcher.Models.Models;
using NewsCatcher.Services.Interfaces;

namespace NewsCatcher.Services.Services
{
    public class UserService : IUserService
    {
        public Task<UsersModel.LoginModel.Return> UserLoginAsync(UsersModel.LoginModel.Request request)
        {
            throw new NotImplementedException();
        }
    }
}
