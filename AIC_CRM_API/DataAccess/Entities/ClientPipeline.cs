using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class ClientPipeline : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public int? ManagerId { get; set; }
        public string? WinningProbabilityAmount { get; set; }
        public string? PipelineAmount { get; set; }
        public string? PipelineSource { get; set; }
        public string pipelineTypes { get; set; } = null!;
        public string? PipelineAssociatedWithReqs { get; set; }
        public int? WinningProbabilities { get; set; }
    }
}
