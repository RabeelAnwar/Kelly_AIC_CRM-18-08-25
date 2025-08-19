using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Client.DTOs
{
    public class ClientPipelineDto
    {
        public int Id { get; set; }
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
