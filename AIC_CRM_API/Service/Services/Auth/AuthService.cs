using System.Data;
using System.Security.Claims;
using System.Text;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.UnitOfWorks;
using Service.Services.Auth.DTOs;
using Utility.OutputData;
using EntityUser = DataAccess.Entities.User;
using Microsoft.IdentityModel.Tokens;
using Service.Services.Company;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Service.Sessions;

namespace Service.Services.Auth
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<EntityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private const string entity = "User";

        public AuthService(UserManager<EntityUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, MainContext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<OutputDTO<UserDetailsDto>> UserLogin(UserLoginDto input)
        {
            var result = new UserDetailsDto();

            var company = await _context.Companies.AsNoTracking().FirstOrDefaultAsync(x => x.CompanyName.ToLower() == input.CompanyId.ToLower() && x.ActiveStatus == true);
            if (company == null)
            {
                return OutputHandler.Handler((int)ResponseType.SESSION_EXIST, result, "The specified company could not be found.");
            }

            var splitUserName = input.Email.Split('-');

            if (splitUserName.Length > 1)
            {
                var splitTenantId = Convert.ToInt32(splitUserName[1]);

                if (splitTenantId != company.TenantId)
                {
                    input.Email = input.Email + "-" + company.TenantId;
                }
            }
            else
            {
                // No tenant ID in email, so append the company's tenant ID
                input.Email = input.Email + "-" + company.TenantId;
            }

            var identityUser = await _context.Users.AsNoTracking()
                                   .Where(u => u.UserName.ToLower() == input.Email.ToLower() && u.TenantId == company.TenantId && u.ActiveStatus == true)
                                   .FirstOrDefaultAsync();

            if (identityUser == null)
            {
                return OutputHandler.Handler((int)ResponseType.SESSION_EXIST, result, "Incorrect username or password");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(identityUser, input.Password);
            if (!passwordValid)
            {
                return OutputHandler.Handler((int)ResponseType.SESSION_EXIST, result, "Incorrect username or password");
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            var roleName = roles.FirstOrDefault();
            var roleEntity = await _roleManager.FindByNameAsync(roleName);

            result.UserName = identityUser.UserName;
            result.Email = identityUser.Email;
            result.PhoneNumber = identityUser.PhoneNumber;
            result.Role = roleName;
            result.TenantId = company.TenantId;
            result.FullName = identityUser.FirstName + " " + identityUser.LastName;
            result.UserId = identityUser.Id;
            SessionModel sessionData = new SessionModel()
            {
                UserName = identityUser.UserName,
                UserId = identityUser.Id,
                UserRole = roleEntity.Name,
                UserRoleId = roleEntity.Id,
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                TenantId = company.TenantId,
            };

            result.Token = CreateJWT(sessionData);

            return OutputHandler.Handler((int)ResponseType.LOGIN_SUCCESS, result, entity);
        }

        private string CreateJWT(SessionModel input)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, input.UserName),
                new Claim("UserId", input.UserId),
                new Claim(ClaimTypes.Role, input.UserRole),
                new Claim("UserRoleId", input.UserRoleId),
                new Claim("CompanyName", input.CompanyName),
                new Claim("CompanyId", input.CompanyId.ToString()),
                new Claim("TenantId", input.TenantId.ToString()),
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(6),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

    }
}
