using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Service.Services.Admin.SupportTicket
{
    public class SupportTicketDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Subject { get; set; }
        public string? Description { get; set; }
        public string? Document { get; set; }
        public IFormFile? DocumentFile { get; set; }
    }
}
