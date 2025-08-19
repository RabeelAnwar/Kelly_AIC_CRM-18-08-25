using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace Service.Services.Client.DTOs
{
    public class ClientInput
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = null!;
        public int? ParentClientId { get; set; }
        public string? CompleteAddress { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? OfficeContact { get; set; }
        public string? OfficeContactExt { get; set; }
        public string? Division { get; set; }
        public string? Website { get; set; }
        public string? PurchaseContactName { get; set; }
        public string? PurchaseContact { get; set; }
        public string? PurchaseContactExt { get; set; }
        public string? DiversityContactName { get; set; }
        public string? DiversityContact { get; set; }
        public string? DiversityContactExt { get; set; }
        public bool? Registered { get; set; }
        public string? RegistrationPortal { get; set; }
        public string? CompanySize { get; set; }
        public string? Notes { get; set; }
    }
}
