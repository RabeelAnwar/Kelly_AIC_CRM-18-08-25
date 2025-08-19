using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Auth.DTOs
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
        public string CompanyId { get; set; }
    }


    public class UserDetailsDto
    {
        public string? UserId{ get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
        public int? TenantId { get; set; }
    }
}
