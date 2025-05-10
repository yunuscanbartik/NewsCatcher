using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsCatcherApi.Models;

namespace NewsCatcher.Services.Interfaces
{
    public interface IUserService
    {
        Task<UsersModel.LoginModel.Return> UserLoginAsync(UsersModel.LoginModel.Request request);
    }
}
