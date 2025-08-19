using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Service.Services.DocumentUpload.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string? DocumentName { get; set; }
        public int DocumentTypeId { get; set; }
        public int? ClientId { get; set; }
        public int? ClientManagerId { get; set; }
        public int? ConsultantId { get; set; }
        public int? RequisitionId { get; set; }
        public string? DocumentTypeName { get; set; }
        public string Source { get; set; }
        public string? DocumentFileName { get; set; }
        public IFormFile? DocumentFile { get; set; }
    }
}
