
using System.Text.Json.Serialization;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class ClientRequisition : FullAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string InternalReqCoordinatorId { get; set; } // Required
        public string RequisitionType { get; set; }       // Required
        public int Priority { get; set; }                 // Required
        public string JobTitle { get; set; }              // Required
        public string? ClientReqNumber { get; set; }
        public int ManagerId { get; set; }                // Required
        public string SalesRepId { get; set; }               // Required
        public List<string> RecruiterAssignedId { get; set; } = new(); // Required
        public string? Location { get; set; }
        public string? Duration { get; set; }
        public string? DurationTypes { get; set; }
        public DateTime StartDate { get; set; }           // Required
        public int NumberOfPositions { get; set; }        // Required
        public string? Comments { get; set; }
        public string? ProjectDepartmentOverview { get; set; }
        public string? JobDescription { get; set; }
        public string? PayRate { get; set; }
        public string? BillRate { get; set; }
        public string? Hours { get; set; }
        public string? Overtime { get; set; }
        public string? InterviewProcesses { get; set; }
        public string? PhoneHireIfOutOfArea { get; set; }
        public string? ClientMarkup { get; set; }
        public string? BillRateHighestBeforeResumeNotSent { get; set; }
        public string? SecondaryContact { get; set; }
        public string? HiringManagerVop { get; set; }
        public string? OtherWaysToFillPosition { get; set; }
        public string? Notes { get; set; }
        public string? Responsibilities { get; set; }
        public string? Qualifications { get; set; }
        public string? SearchString1 { get; set; }
        public string? SearchString2 { get; set; }
        public string? SearchString3 { get; set; }
        public int? CodingValue { get; set; }
        public int? AnalysisValue { get; set; }
        public int? TestingValue { get; set; }
        public int? OtherValue { get; set; }

        // Technical Skills Section
        public string? Hardware { get; set; }
        public string? OS { get; set; }
        public string? Languages { get; set; }
        public string? Databases { get; set; }
        public string? Protocols { get; set; }
        public string? SoftwareStandards { get; set; }
        public string? Others { get; set; }
        public bool? Status { get; set; }
    }
}
