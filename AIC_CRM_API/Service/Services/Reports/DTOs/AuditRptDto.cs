using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Reports.DTOs
{
    public class AuditRptDto
    {

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? Action { get; set; }
        public string? FormName { get; set; }
        public string? FormId { get; set; }
        public string? Message { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatorId { get; set; }
        public DateTime? CreationTime { get; set; }
    }
}
