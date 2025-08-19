using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Auth.DTOs;
using Utility.OutputData;

namespace Service.Services.Auth
{
    public interface IAuthService
    {
        Task<OutputDTO<UserDetailsDto>> UserLogin(UserLoginDto user);
    }
}
