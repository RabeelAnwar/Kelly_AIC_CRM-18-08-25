using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Service.Services.Consultant.DTOs
{
    public class ConsultantDto
    {
        public int Id { get; set; }
        public string? AssignedToId { get; set; }

        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }

        public string? CurrentPosition { get; set; }
        public string? VisaStatus { get; set; }
        public string? CurrentRate { get; set; }

        public IFormFile? ResumeFile { get; set; }
        public string? Resume { get; set; }
        public string? ResumeSearchText { get; set; }
        public string? CompleteAddress { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }

        public string? OfficePhone { get; set; }
        public string? OfficePhoneExt { get; set; }

        public string? CellPhone { get; set; }
        public string? CellPhoneExt { get; set; }

        public string? HomePhone { get; set; }
        public string? HomePhoneExt { get; set; }

        public string? WorkEmail { get; set; }
        public string? PersonalEmail { get; set; }
        public string? SkypeId { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? LinkedInImage { get; set; }

        public string? Notes { get; set; }
        public string? CallRecords { get; set; }
        public DateTime? CreationTime { get; set; }
        public List<ConsultantRequisitionsDto>? Requisition { get; set; }
    }

    public record ConsultantRequisitionsDto
    {
        public string ClientName { get; set; }
        public int RequisitionId { get; set; }
    }
}
