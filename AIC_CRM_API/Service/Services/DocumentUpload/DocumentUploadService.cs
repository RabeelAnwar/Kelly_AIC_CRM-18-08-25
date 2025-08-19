using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DocumentEntity = DataAccess.Entities.DocumentUpload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.FileUpload;
using Service.Services.DocumentUpload.DTOs;
using Service.Sessions;
using Utility.OutputData;

namespace Service.Services.DocumentUpload
{
    public class DocumentUploadService : IDocumentUploadService
    {

        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IFileUpload _fileUpload;


        public DocumentUploadService(MainContext context, IMapper mapper, ISessionData sessionData, IWebHostEnvironment webHostEnvironment, IFileUpload fileUpload)
        {
            _context = context;
            _mapper = mapper;
            _session = sessionData;
            _hostingEnvironment = webHostEnvironment;
            _fileUpload = fileUpload;
        }

        public async Task<OutputDTO<DocumentDto>> DocumentAddUpdate(DocumentDto input)
        {
            try
            {
                var existingDocument = await _context.DocumentUploads
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedDocument = _mapper.Map<DocumentEntity>(input);
                mappedDocument.TenantId = _session.TenantId;
                var filePath = string.Empty;

                if (input.DocumentFile != null)
                {
                    filePath = Path.Combine("Tenants", _session.TenantId.ToString(), input.Source, input.DocumentFile.FileName).Replace("\\", "/");
                    mappedDocument.DocumentFileName = filePath;
                }

                DocumentEntity savedDocument;

                if (existingDocument != null)
                {
                    var updatedEntity = _mapper.Map(input, existingDocument);
                    updatedEntity.DocumentFileName = filePath;
                    updatedEntity.TenantId = _session.TenantId;

                    _context.DocumentUploads.Update(updatedEntity);
                    await _context.SaveChangesAsync();
                    savedDocument = updatedEntity;
                }
                else
                {
                    _context.DocumentUploads.Add(mappedDocument);
                    await _context.SaveChangesAsync();
                    savedDocument = mappedDocument;
                }

                if (input.DocumentFile != null)
                {
                    _fileUpload.fileUpload(input.DocumentFile, _session.TenantId.ToString(), input.Source, input.DocumentFile.FileName, _hostingEnvironment);
                }

                var resultDto = _mapper.Map<DocumentDto>(savedDocument);
                return OutputHandler.Handler((int)(existingDocument != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, input.Source.ToUpper());
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<OutputDTO<bool>> DocumentDelete(int id)
        {
            try
            {
                var document = await _context.DocumentUploads
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (document == null)
                {
                    return OutputHandler.Handler<bool>((int)ResponseType.NOT_EXIST, false, "Document not found");
                }

                if (string.IsNullOrWhiteSpace(document.DocumentFileName))
                {
                    return OutputHandler.Handler<bool>((int)ResponseType.NOT_EXIST, false, "No document found to delete");
                }

                // Construct the full path
                var fileFullPath = Path.Combine(
                    _hostingEnvironment.WebRootPath,
                    document.DocumentFileName.Replace("/", Path.DirectorySeparatorChar.ToString())
                );

                if (File.Exists(fileFullPath))
                {
                    File.Delete(fileFullPath);
                }

                _context.DocumentUploads.Remove(document);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Deleted successfully");
            }
            catch (Exception ex)
            {
                return OutputHandler.Handler<bool>((int)ResponseType.SESSION_EXIST, false, $"An error occurred: {ex.Message}");
            }
        }


        //public async Task<OutputDTO<List<DocumentDto>>> DocumentsListGet(int clientId, int managerId, int consultantId, int requisitionId, string source)
        //{
        //    try
        //    {
        //        var query = from doc in _context.DocumentUploads
        //                    join docType in _context.DocumentTypes
        //                        on doc.DocumentTypeId equals docType.Id
        //                    where doc.TenantId == _session.TenantId
        //                    select new DocumentDto
        //                    {
        //                        Id = doc.Id,
        //                        DocumentName = doc.DocumentName,
        //                        DocumentTypeId = doc.DocumentTypeId,
        //                        DocumentTypeName = docType.DocumentTypeName,
        //                        ClientId = doc.ClientId,
        //                        ClientManagerId = doc.ClientManagerId,
        //                        ConsultantId = doc.ConsultantId,
        //                        Source = doc.Source,
        //                        DocumentFileName = doc.DocumentFileName,
        //                        // Include other mappings as needed
        //                    };

        //        // Apply filtering based on 'source'
        //        switch (source?.ToLower())
        //        {
        //            case "client":
        //                query = query.Where(doc => doc.ClientId == clientId);
        //                break;

        //            case "manager":
        //                query = query.Where(doc => doc.ClientManagerId == managerId && doc.ClientId == clientId);
        //                break;

        //            case "consultant":
        //                query = query.Where(doc => doc.ConsultantId == consultantId);
        //                break;

        //            case "requisition":
        //                query = query.Where(doc => doc.RequisitionId == requisitionId);
        //                break;

        //            default:
        //                // Optional: throw exception or handle unknown source
        //                throw new ArgumentException("Invalid source specified.");
        //        }

        //        var documents = await query.ToListAsync();
        //        var mappedDocuments = _mapper.Map<List<DocumentDto>>(documents);

        //        return OutputHandler.Handler((int)ResponseType.GET, mappedDocuments, "Documents", mappedDocuments.Count);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        public async Task<OutputDTO<List<DocumentDto>>> DocumentsListGet(int clientId, int managerId, int consultantId, int requisitionId, string source)
        {
            try
            {
                var query = from doc in _context.DocumentUploads
                            join docType in _context.DocumentTypes
                                on doc.DocumentTypeId equals docType.Id
                            where doc.TenantId == _session.TenantId
                            select new { doc, docType };

                // Apply filtering based on 'source'
                switch (source?.ToLower())
                {
                    case "client":
                        query = query.Where(x => x.doc.ClientId == clientId);
                        break;

                    case "manager":
                        query = query.Where(x => x.doc.ClientManagerId == managerId && x.doc.ClientId == clientId);
                        break;

                    case "consultant":
                        query = query.Where(x => x.doc.ConsultantId == consultantId);
                        break;

                    case "requisition":
                        query = query.Where(x => x.doc.RequisitionId == requisitionId);
                        break;

                    default:
                        throw new ArgumentException("Invalid source specified.");
                }

                var documents = await query
                    .Select(x => new DocumentDto
                    {
                        Id = x.doc.Id,
                        DocumentName = x.doc.DocumentName,
                        DocumentTypeId = x.doc.DocumentTypeId,
                        DocumentTypeName = x.docType.DocumentTypeName,
                        ClientId = x.doc.ClientId,
                        ClientManagerId = x.doc.ClientManagerId,
                        ConsultantId = x.doc.ConsultantId,
                        Source = x.doc.Source,
                        DocumentFileName = x.doc.DocumentFileName,
                        RequisitionId = x.doc.RequisitionId,
                    })
                    .ToListAsync();

                // Optional: Only map if additional mapping logic is needed
                var mappedDocuments = _mapper.Map<List<DocumentDto>>(documents);

                return OutputHandler.Handler((int)ResponseType.GET, mappedDocuments, "Documents", mappedDocuments.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
