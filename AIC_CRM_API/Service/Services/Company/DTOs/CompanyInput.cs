using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace Service.Services.Company.DTOs
{
    public class CompanyInput
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }
        public int? TenantId { get; set; }

        public bool? ActiveStatus { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? Country { get; set; }

        public string? State { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public string? PhoneNo1 { get; set; }

        public string? PhoneNo1Ext { get; set; }

        public string? PhoneNo2 { get; set; }

        public string? AlternatePhoneNo { get; set; }

        public string? WorkEmailId { get; set; }

        public string? PersonalEmailId { get; set; }

        public string? SkypeId { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }
    }
}
