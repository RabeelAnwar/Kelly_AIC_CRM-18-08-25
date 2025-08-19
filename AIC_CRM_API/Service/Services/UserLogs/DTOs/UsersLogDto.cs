using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UserLogs.DTOs
{
    public class UsersLogDto
    {
        public string Action { get; set; }
        public string FormName { get; set; }
        public string FormId { get; set; }
        public string Message { get; set; }
    }
}
