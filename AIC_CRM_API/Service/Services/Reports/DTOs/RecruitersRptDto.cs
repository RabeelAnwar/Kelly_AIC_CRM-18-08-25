using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Reports.DTOs
{
    public class RecruitersRptDto
    {

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }


        public string? Recruiter { get; set; }
        public int? Submittals { get; set; }
        public int? Interviews { get; set; }
        public int? Starts { get; set; }
        public int? ConsultantsAdded { get; set; }
        public int? CallLogsAdded { get; set; }
        public List<string>? RequisitionsDetails { get; set; }
    }
}
