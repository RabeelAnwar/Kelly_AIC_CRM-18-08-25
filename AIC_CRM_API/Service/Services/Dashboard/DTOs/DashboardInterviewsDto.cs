using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Dashboard.DTOs
{
    public class DashboardInterviewsDto
    {
        public string? ConsultantName { get; set; }
        public int? ConsultantId { get; set; }
        public string? Client { get; set; }
        public int? ClientId { get; set; }
        public string? ReqNo { get; set; }
        public int? ReqId { get; set; }
        public decimal? BillRate { get; set; }
        public decimal? PayRate { get; set; }
        public string? Markup { get; set; }
        public DateTime? InterviewTime { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string? Notes { get; set; }

    }
}
