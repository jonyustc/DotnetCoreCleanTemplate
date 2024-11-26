using Clean.Application.Bases;
using Clean.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Application.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResponse<LoginResult>> Login(LoginCommand request);
        Task<BaseResponse<RegisterResult>> Register(RegisterCommand request);
    }
}
