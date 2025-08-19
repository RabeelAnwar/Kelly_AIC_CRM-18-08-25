using DataAccess.Entities;
using UserEntity = DataAccess.Entities.User;
using Microsoft.AspNetCore.Identity;
using Service.Services.User.UserMaster;
using Service.Sessions;
using Utility.OutputData;
using Microsoft.EntityFrameworkCore;

namespace Service.Services.User
{
    public class UserMasterService : IUserMasterService
    {
        private readonly IMapper _mapper;
        private readonly ISessionData _session;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string entity = "User";

        public UserMasterService(
            IMapper mapper,
            ISessionData sessionData,
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _session = sessionData;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Create or Update User
        public async Task<OutputDTO<bool>> UserAddUpdate(UserMasterDto input)
        {
            try
            {
                UserEntity user;
                input.UserPassword ??= input.Password;
                bool isUpdate = !string.IsNullOrEmpty(input.Id);

                if (isUpdate)
                {
                    // Update existing user
                    user = await _userManager.FindByIdAsync(input.Id.ToString());
                    if (user == null)
                        return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, entity);

                    // Check if email is changed and already exists
                    if (user.UserName != input.UserName && await _userManager.FindByNameAsync(input.UserName) != null)
                        return OutputHandler.Handler((int)ResponseType.EXIST, false, entity);
                }
                else
                {
                    // Create new user
                    var existingUser = await _userManager.Users
                        .Where(u => u.UserName == input.UserName && u.TenantId == (input.TenantId ?? _session.TenantId))
                        .FirstOrDefaultAsync();

                    if (existingUser != null)
                        return OutputHandler.Handler((int)ResponseType.EXIST, false, entity);

                    user = _mapper.Map<UserEntity>(input);
                    user.CreationTime = DateTime.UtcNow;
                    user.CreatorUserId = _session.UserId;
                }

                // Update common fields
                _mapper.Map(input, user); // Update fields from DTO
                user.LastModificationTime = DateTime.UtcNow;
                user.LastModifierUserId = _session.UserId;
                user.TenantId = input.TenantId ?? _session.TenantId;
                user.ActiveStatus = input.ActiveStatus;

                // Validate role
                if (!await _roleManager.RoleExistsAsync(input.ContactTypeName))
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Role");

                IdentityResult result;
                if (isUpdate)
                {
                    result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        // Update roles
                        var currentRoles = await _userManager.GetRolesAsync(user);
                        if (!currentRoles.Contains(input.ContactTypeName))
                        {
                            await _userManager.RemoveFromRolesAsync(user, currentRoles);
                            result = await _userManager.AddToRoleAsync(user, input.ContactTypeName);
                        }
                    }
                }
                else
                {
                    user.UserName = user.UserName + "-" + user.TenantId;
                    result = await _userManager.CreateAsync(user, input.Password);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRoleAsync(user, input.ContactTypeName);
                    }
                }

                if (!result.Succeeded)
                {
                    var error = result.Errors.FirstOrDefault()?.Description ?? "Operation failed";
                    return OutputHandler.Handler((int)ResponseType.SESSION_EXIST, false, error);
                }

                return OutputHandler.Handler((int)(isUpdate ? ResponseType.UPDATE : ResponseType.CREATE), true, entity);
            }
            catch (Exception ex)
            {
                // Log exception (use your logging framework)
                return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, ex.Message);
            }
        }

        // Delete: Soft Delete User
        public async Task<OutputDTO<bool>> UserDelete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null || user.IsDeleted)
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, entity);

                user.IsDeleted = true;
                user.DeletionTime = DateTime.UtcNow;
                user.DeleterUserId = _session.UserId;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var error = result.Errors.FirstOrDefault()?.Description ?? "Deletion failed";
                    return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, error);
                }

                return OutputHandler.Handler((int)ResponseType.DELETE, true, entity);
            }
            catch (Exception ex)
            {
                return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, ex.Message);
            }
        }

        // Read: Get All Users
        public async Task<OutputDTO<List<UserMasterDto>>> UsersListGet()
        {
            try
            {
                var users = await _userManager.Users
                    .Include(u => u.ContactType)
                    .Where(u => !u.IsDeleted && u.TenantId == _session.TenantId)
                    .ToListAsync();

                var userDtos = new List<UserMasterDto>();
                foreach (var user in users)
                {
                    var dto = new UserMasterDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        MiddleName = user.MiddleName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Password = user.UserPassword,
                        ContactTypeId = user.ContactTypeId,
                        Address1 = user.Address1,
                        Address2 = user.Address2,
                        Country = user.Country,
                        State = user.State,
                        City = user.City,
                        ZipCode = user.ZipCode,
                        Phone1 = user.Phone1,
                        Phone1Ext = user.Phone1Ext,
                        Phone2 = user.Phone2,
                        Phone2Ext = user.Phone2Ext,
                        AlternatePhone = user.AlternatePhone,
                        AlternatePhoneExt = user.AlternatePhoneExt,
                        WorkEmail = user.WorkEmail,
                        PersonalEmail = user.PersonalEmail,
                        SkypeId = user.SkypeId,
                        ActiveStatus = user.ActiveStatus,
                        TenantId = user.TenantId,
                        CreatorUserId = user.CreatorUserId,
                        CreationTime = user.CreationTime,
                        LastModifierUserId = user.LastModifierUserId,
                        LastModificationTime = user.LastModificationTime,
                        IsDeleted = user.IsDeleted,
                        DeleterUserId = user.DeleterUserId,
                        DeletionTime = user.DeletionTime
                    };

                    dto.ContactTypeName = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty;
                    userDtos.Add(dto);
                }

                return OutputHandler.Handler((int)ResponseType.GET_ALL, userDtos, entity, userDtos.Count);
            }
            catch (Exception ex)
            {
                return OutputHandler.Handler<List<UserMasterDto>>((int)ResponseType.INVALID_REQUEST, null, ex.Message, 0);
            }
        }

        // Read: Get User by ID
        public async Task<OutputDTO<UserMasterDto>> UserProfileGet(string id)
        {
            try
            {
                var user = await _userManager.Users
                    .Include(u => u.ContactType)
                    .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted && u.TenantId == _session.TenantId);

                if (user == null)
                    return OutputHandler.Handler<UserMasterDto>((int)ResponseType.NOT_EXIST, null, entity, 0);

                var userDto = _mapper.Map<UserMasterDto>(user);
                userDto.ContactTypeName = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty;

                return OutputHandler.Handler((int)ResponseType.GET, userDto, entity, 0);
            }
            catch (Exception ex)
            {
                return OutputHandler.Handler<UserMasterDto>((int)ResponseType.INVALID_REQUEST, null, ex.Message, 0);
            }
        }

        // Forgot Password: Initiate Password Reset
        //public async Task<OutputDTO<bool>> ForgotPassword(string email)
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByEmailAsync(email);
        //        if (user == null || user.IsDeleted)
        //            return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "User with this email");

        //        // Generate password reset token
        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //        // Note: In a real application, send the token via email to the user
        //        // For this example, we'll assume the token is handled externally
        //        // Example: await _emailService.SendPasswordResetEmail(user.Email, token);

        //        return OutputHandler.Handler((int)ResponseType.Reset, true, "Password reset token generated");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, ex.Message);
        //    }
        //}


        // Reset Password: Using Token
        //public async Task<OutputDTO<bool>> ResetPassword(string email, string token, string newPassword)
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByEmailAsync(email);
        //        if (user == null || user.IsDeleted)
        //            return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "User with this email");

        //        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        //        if (!result.Succeeded)
        //        {
        //            var error = result.Errors.FirstOrDefault()?.Description ?? "Password reset failed";
        //            return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, error);
        //        }

        //        return OutputHandler.Handler((int)ResponseType.SUCCESS, true, "Password reset successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, ex.Message);
        //    }
        //}




    }
}
