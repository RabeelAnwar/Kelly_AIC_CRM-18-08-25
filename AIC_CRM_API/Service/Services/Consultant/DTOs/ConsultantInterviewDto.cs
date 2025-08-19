using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Consultant.DTOs
{
    public class ConsultantInterviewDto
    {
        public int Id { get; set; }
        public int ConsultantId { get; set; }
        public string? Requisition { get; set; }
        public string? InterviewByStaff { get; set; }
        public string? InterviewByNonStaff { get; set; }
        public string? Comments { get; set; }
        public string? InterviewResults { get; set; }
    }

}
