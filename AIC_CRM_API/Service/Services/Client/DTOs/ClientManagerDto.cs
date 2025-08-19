using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Client.DTOs
{
    public class ClientManagerDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string? Title { get; set; }
        public int? DepartmentId { get; set; }
        public int? WorksUnderId { get; set; }
        public string? IsAssignedToId { get; set; }

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
        public string? LinkedInUrl { get; set; }
        public string? LinkedInImageUrl { get; set; }

        public string? SkillNeeded { get; set; }
        public string? Notes { get; set; }
        public bool? IsManager { get; set; }
        public bool? StillWithCompany { get; set; }
        public bool? IsActive { get; set; }
        public string? CallRecords { get; set; }

    }
}
