using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Reports.DTOs
{
    public class MasterOfficeRptDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<RequisitionActivity>? RequisitionActivities { get; set; }
        public List<RequisitionInterviews>? RequisitionInterviews { get; set; }
    }


    public class RequisitionActivity
    {
        public DateTime CreatedDate { get; set; }
        public string Coordinator { get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public string ReqName { get; set; }
        public int ReqId { get; set; }
        public string ManagerName { get; set; }
        public int ManagerId { get; set; }
        public int Submittals { get; set; }
        public int Interviews { get; set; }
        public int Starts { get; set; }
        public int NoOfPosition { get; set; }
        public string BillRate { get; set; }
        public string Duration { get; set; }
    }


    public class RequisitionInterviews
    {
        public DateTime EnteredDate { get; set; }
        public string SalesRep { get; set; }
        public string Recruiter { get; set; }
        public string ConsultantName { get; set; }
        public int ConsultantId{ get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public string Requisition { get; set; }
        public int RequisitionId { get; set; }
        public DateTime? StartDate { get; set; }
        public decimal BillRate { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal Markup { get; set; }
    }
}
