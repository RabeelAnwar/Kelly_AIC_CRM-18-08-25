using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Consultant.DTOs
{
    public class ConsultantActivityDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ManagerId { get; set; }
        public int ConsultantId { get; set; }
        public string? ConsultantName { get; set; }
        public int RequisitionId { get; set; }
        public string AssignedToId { get; set; }
        public decimal? BillRate { get; set; }
        public decimal? PayRate { get; set; }
        public DateTime? LastContact { get; set; }
        public DateTime? CreationTime { get; set; }
        public bool? Disabled { get; set; }

        public List<string>? InterviewStatus { get; set; }
        public List<DateTime>? InterviewDateTime { get; set; }
    }

}
