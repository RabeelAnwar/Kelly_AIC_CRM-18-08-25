using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Lead.DTOs
{
    public class LeadInput
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Category { get; set; }
        public string LeadType { get; set; }
        public string StatusOfLead { get; set; }
        public int DepartmentId { get; set; }
        public int ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerTitle { get; set; }
        public string AssignedToId { get; set; }
        public string? AssignedTo { get; set; }
        public string? GeneratedById { get; set; }
        public string? GeneratedBy { get; set; }
        public bool? Called { get; set; }
        public bool? IsStaffConsultant { get; set; }
        public bool? IsClientManager { get; set; }
        public string? ReferredBy { get; set; }
        public decimal? ApproximateAmount { get; set; }
        public string? Source { get; set; }
        public string? LeadInformation { get; set; }
        public string? Result { get; set; }
        public DateTime? ReminderDateTime { get; set; }
    }
}
