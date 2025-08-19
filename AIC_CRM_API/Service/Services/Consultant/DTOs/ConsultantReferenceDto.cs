using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Consultant.DTOs
{
    public class ConsultantReferenceDto
    {
        public int Id { get; set; }
        public int ConsultantId { get; set; }
        public string? ReferenceName { get; set; }
        public string? Company { get; set; }
        public string? ManagerPhone { get; set; }
        public string? ManagerEmail { get; set; }
        public DateTime? ReferenceDate { get; set; } = DateTime.Now;
        public string? EmployementDates { get; set; }
        public string? HiringManagerComments { get; set; }
    }

}
