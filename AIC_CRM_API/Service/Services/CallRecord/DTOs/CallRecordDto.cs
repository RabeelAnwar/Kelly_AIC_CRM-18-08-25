using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.CallRecord.DTOs
{
    public class CallRecordDto
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int? LeadId { get; set; }
        public int? ManagerId { get; set; }
        public int? ConsultantId { get; set; }
        public DateTime Date { get; set; }
        public int? TypeId { get; set; }
        public string? Record { get; set; }
        public bool? RemindStatus { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string? CreatorUserName { get; set; }
    }
}
