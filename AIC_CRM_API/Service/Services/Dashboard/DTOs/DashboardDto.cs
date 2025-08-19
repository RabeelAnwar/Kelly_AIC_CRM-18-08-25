using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Dashboard.DTOs
{
    public class DashboardDto
    {
        public int RecentClientCallRecords { get; set; }
        public int RecentConsultantCallRecords { get; set; }
        public int Activities { get; set; }
        public int RecentRequisitions { get; set; }
        public int MyLeads { get; set; }
        public int ReminderCalls { get; set; }
        public int SupportTickets { get; set; }
        public int TodayCallRecords { get; set; }
        public int TodayRequisitionsTotal { get; set; }
        public int TodaySubmitalsTotal { get; set; }
    }
}
