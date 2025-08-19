using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.DocumentUpload.DTOs;
using Utility.OutputData;

namespace Service.Services.DocumentUpload
{
    public interface IDocumentUploadService
    {
        Task<OutputDTO<DocumentDto>> DocumentAddUpdate(DocumentDto input);

        Task<OutputDTO<bool>> DocumentDelete(int id);
        Task<OutputDTO<List<DocumentDto>>> DocumentsListGet(int clientId, int managerId, int consultantId, int requisitionId, string source);

    }
}
