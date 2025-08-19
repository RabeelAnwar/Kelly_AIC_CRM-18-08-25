
using Service.Services.User.UserMaster;
using Utility.OutputData;

namespace Service.Services.User
{
    public interface IUserMasterService
    {
        Task<OutputDTO<bool>> UserAddUpdate(UserMasterDto user);
        Task<OutputDTO<bool>> UserDelete(string id);
        Task<OutputDTO<List<UserMasterDto>>> UsersListGet();
        Task<OutputDTO<UserMasterDto>> UserProfileGet(string id);

    }
}
