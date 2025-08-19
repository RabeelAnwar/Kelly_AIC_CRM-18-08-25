
namespace Service.Services.Consultant.DTOs
{
    public class ConsultantInterviewProcessDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int? ConsultantActivityId { get; set; }

        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? EmploymentType { get; set; }
        public decimal? Salary { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? Expenses { get; set; }
        public decimal? LoadedRate { get; set; }
        public decimal? BillRate { get; set; }
        public decimal? Vop { get; set; }
        public decimal? Markup { get; set; }
        public string? RecruiterAssigned { get; set; }
        public string? SalesRep { get; set; }
        public string? NotesDetail { get; set; }
        public bool? StartCandidate { get; set; }
    }
}
