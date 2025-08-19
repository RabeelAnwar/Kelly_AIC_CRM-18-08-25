using DataAccess.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Converter;
using Service.FileUpload;
using Service.Services.Consultant.DTOs;
using Service.Services.Requisition.DTOs;
using Service.Services.UserLogs;
using Service.Services.UserLogs.DTOs;
using Service.Sessions;
using System.Net.Http.Headers;
using Utility.OutputData;
using ConsultantEntity = DataAccess.Entities.Consultant;

namespace Service.Services.Consultant
{
    public class ConsultantService : IConsultantService
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;
        private readonly IFileUpload _fileUpload;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly HttpClient _httpClient;

        public ConsultantService(MainContext context, IMapper mapper, ISessionData sessionData, IFileUpload file, IWebHostEnvironment hostEnvironment)
        {

            _httpClient = new HttpClient();
           // _httpClient.BaseAddress = new Uri("http://172.235.58.249:9998/");
            _httpClient.BaseAddress = new Uri("http://http://172.233.152.32:8080/");
        

            _context = context;
            _mapper = mapper;
            _session = sessionData;
            _fileUpload = file;
            _hostingEnvironment = hostEnvironment;
        }



        public async Task<OutputDTO<ConsultantDto>> ConsultantAddUpdate(ConsultantDto input)
        {
            try
            {
                var existingConsultant = await _context.Consultants
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedConsultant = _mapper.Map<ConsultantEntity>(input);
                mappedConsultant.TenantId = _session.TenantId;


                var resumePath = string.Empty;

                if (input.ResumeFile != null)
                {
                    resumePath = Path.Combine("Tenants", _session.TenantId.ToString(), "Consultant", input.ResumeFile.FileName).Replace("\\", "/");
                    mappedConsultant.Resume = resumePath;
                }

                ConsultantEntity savedConsultant;

                if (existingConsultant != null)
                {
                    //var updatedEntity = _mapper.Map(input, existingConsultant);
                    ////    if (input.ResumeFile != null)
                    //if (!string.IsNullOrEmpty(resumePath))
                    //{
                    //    // resumePath = Path.Combine("Tenants", _session.TenantId.ToString(), "Consultant", input.ResumeFile.FileName).Replace("\\", "/");
                    //    ///updatedEntity.Resume = resumePath;

                    //    updatedEntity.Resume = resumePath;
                    //}
                    //else
                    //{
                    //    updatedEntity.Resume = existingConsultant.Resume; // retain old resume
                    //}
                    //updatedEntity.TenantId = _session.TenantId;

                    //_context.Consultants.Update(updatedEntity);
                    //await _context.SaveChangesAsync();
                    //savedConsultant = updatedEntity;

                    var originalResume = existingConsultant.Resume;
                    var updatedEntity = _mapper.Map(input, existingConsultant);
                    if (!string.IsNullOrEmpty(resumePath))
                    {
                        updatedEntity.Resume = resumePath;
                    }
                    else
                    {
                        updatedEntity.Resume = originalResume;
                    }

                    updatedEntity.TenantId = _session.TenantId;

                    _context.Consultants.Update(updatedEntity);
                    await _context.SaveChangesAsync();
                    savedConsultant = updatedEntity;
                }
                else
                {
                    _context.Consultants.Add(mappedConsultant);
                    await _context.SaveChangesAsync();
                    savedConsultant = mappedConsultant;
                }

                if (input.ResumeFile != null)
                {
                    // Upload resume file after save
                    _fileUpload.fileUpload(input.ResumeFile, _session.TenantId.ToString(), "Consultant", input.ResumeFile.FileName, _hostingEnvironment);
                }

                var logInput = new UsersLogDto
                {
                    Action = existingConsultant != null ? "Update" : "Create",
                    FormName = "Consultant",
                    FormId = savedConsultant.Id.ToString(),
                    Message = $"{(existingConsultant != null ? "Updated" : "Created")} Consultant  {savedConsultant.FirstName} {savedConsultant.LastName}"
                };
                await _context.SaveUserLogAsync(logInput, _session, _mapper);


                var resultDto = _mapper.Map<ConsultantDto>(savedConsultant);
                return OutputHandler.Handler((int)(existingConsultant != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, "Consultant");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpPost]
        //public async Task<OutputDTO<ConsultantDto>> ConsultantAddUpdate([FromForm] ConsultantDto input)
        //{
        //    try
        //    {
        //        var existingConsultant = await _context.Consultants
        //            .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

        //        string? resumePath = null;

        //        if (input.ResumeFile != null)
        //        {
        //            resumePath = Path.Combine("Tenants", _session.TenantId.ToString(), "Consultant", input.ResumeFile.FileName).Replace("\\", "/");
        //        }

        //        ConsultantEntity savedConsultant;

        //        if (existingConsultant != null)
        //        {
        //            // UPDATE CASE
        //            var updatedEntity = _mapper.Map(input, existingConsultant);

        //            updatedEntity.Resume = resumePath ?? existingConsultant.Resume; // ✅ keep old resume if no new file
        //            updatedEntity.TenantId = _session.TenantId;

        //            _context.Consultants.Update(updatedEntity);
        //            await _context.SaveChangesAsync();

        //            savedConsultant = updatedEntity;
        //        }
        //        else
        //        {
        //            // CREATE CASE
        //            var mappedConsultant = _mapper.Map<ConsultantEntity>(input);
        //            mappedConsultant.TenantId = _session.TenantId;

        //            if (!string.IsNullOrEmpty(resumePath))
        //            {
        //                mappedConsultant.Resume = resumePath;
        //            }

        //            _context.Consultants.Add(mappedConsultant);
        //            await _context.SaveChangesAsync();

        //            savedConsultant = mappedConsultant;
        //        }

        //        // ✅ Upload file only if new one is selected
        //        if (input.ResumeFile != null)
        //        {
        //            _fileUpload.fileUpload(input.ResumeFile, _session.TenantId.ToString(), "Consultant", input.ResumeFile.FileName, _hostingEnvironment);
        //        }

        //        // ✅ Logging
        //        var logInput = new UsersLogDto
        //        {
        //            Action = existingConsultant != null ? "Update" : "Create",
        //            FormName = "Consultant",
        //            FormId = savedConsultant.Id.ToString(),
        //            Message = $"{(existingConsultant != null ? "Updated" : "Created")} Consultant  {savedConsultant.FirstName} {savedConsultant.LastName}"
        //        };

        //        await _context.SaveUserLogAsync(logInput, _session, _mapper);

        //        var resultDto = _mapper.Map<ConsultantDto>(savedConsultant);
        //        return OutputHandler.Handler((int)(existingConsultant != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, "Consultant");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Always catch & return a proper message
        //        return OutputHandler.Handler<ConsultantDto>(500, null, ex.Message);
        //    }
        //}



        //public async Task<OutputDTO<ConsultantDto>> ConsultantAddUpdate(ConsultantDto input)
        //{
        //    try
        //    {
        //        // Check for existing consultant based on Id and TenantId
        //        var existingConsultant = await _context.Consultants
        //            .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

        //        // Map input DTO to entity
        //        var consultantEntity = _mapper.Map<ConsultantEntity>(input);
        //        consultantEntity.TenantId = _session.TenantId;

        //        // Handle resume file path
        //        string resumePath = null;
        //        if (input.ResumeFile != null)
        //        {
        //            resumePath = Path.Combine("Tenants", _session.TenantId.ToString(), "Consultant", input.ResumeFile.FileName).Replace("\\", "/");
        //            consultantEntity.Resume = resumePath;
        //        }

        //        ConsultantEntity savedConsultant;

        //        if (existingConsultant != null)
        //        {
        //            // Update existing
        //            _mapper.Map(input, existingConsultant);
        //            existingConsultant.Resume = resumePath ?? existingConsultant.Resume; // Only update if new resume is uploaded
        //            existingConsultant.TenantId = _session.TenantId;

        //            _context.Consultants.Update(existingConsultant);
        //            savedConsultant = existingConsultant;
        //        }
        //        else
        //        {
        //            // New record
        //            _context.Consultants.Add(consultantEntity);
        //            savedConsultant = consultantEntity;
        //        }

        //        // Save DB changes
        //        await _context.SaveChangesAsync();

        //        // Upload file after saving DB record
        //        if (input.ResumeFile != null)
        //        {
        //            _fileUpload.fileUpload(input.ResumeFile, _session.TenantId.ToString(), "Consultant", input.ResumeFile.FileName, _hostingEnvironment);
        //        }

        //        // Add log
        //        var logInput = new UsersLogDto
        //        {
        //            Action = existingConsultant != null ? "Update" : "Create",
        //            FormName = "Consultant",
        //            FormId = savedConsultant.Id.ToString(),
        //            Message = $"{(existingConsultant != null ? "Updated" : "Created")} Consultant {savedConsultant.FirstName} {savedConsultant.LastName}"
        //        };

        //        await _context.SaveUserLogAsync(logInput, _session, _mapper);

        //        var resultDto = _mapper.Map<ConsultantDto>(savedConsultant);

        //        return OutputHandler.Handler((int)(existingConsultant != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, "Consultant");
        //    }
        //    catch (Exception ex)
        //    {
        //        // You can optionally log the exception here
        //        throw;
        //    }
        //}


        public async Task<OutputDTO<bool>> ConsultantDelete(int id)
        {
            try
            {
                var consultant = await _context.Consultants.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);
                if (consultant == null) return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Consultant");

                _context.Consultants.Remove(consultant);
                await _context.SaveChangesAsync();

                var logInput = new UsersLogDto
                {
                    Action = "Delete",
                    FormName = "Consultant",
                    FormId = id.ToString(),
                    Message = $"Delete Consultant {consultant.FirstName} {consultant.LastName}"
                };
                await _context.SaveUserLogAsync(logInput, _session, _mapper);

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Consultant");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<ConsultantDto>>> ConsultantsListGet()
        {
            try
            {
                var tenantId = _session.TenantId;

                var result = await (from consultant in _context.Consultants
                                    where consultant.TenantId == tenantId
                                    select new ConsultantDto
                                    {
                                        Id = consultant.Id,
                                        AssignedToId = consultant.AssignedToId,
                                        FirstName = consultant.FirstName,
                                        MiddleName = consultant.MiddleName,
                                        LastName = consultant.LastName,
                                        CurrentPosition = consultant.CurrentPosition,
                                        VisaStatus = consultant.VisaStatus,
                                        CurrentRate = consultant.CurrentRate,
                                        Resume = consultant.Resume,
                                        Address1 = consultant.Address1,
                                        Address2 = consultant.Address2,
                                        Country = consultant.Country,
                                        State = consultant.State,
                                        City = consultant.City,
                                        ZipCode = consultant.ZipCode,
                                        OfficePhone = consultant.OfficePhone,
                                        OfficePhoneExt = consultant.OfficePhoneExt,
                                        CellPhone = consultant.CellPhone,
                                        CellPhoneExt = consultant.CellPhoneExt,
                                        HomePhone = consultant.HomePhone,
                                        HomePhoneExt = consultant.HomePhoneExt,
                                        WorkEmail = consultant.WorkEmail,
                                        PersonalEmail = consultant.PersonalEmail,
                                        SkypeId = consultant.SkypeId,
                                        LinkedInUrl = consultant.LinkedInUrl,
                                        LinkedInImage = consultant.LinkedInImage,
                                        Notes = consultant.Notes,
                                        CallRecords = _context.CallRecords
                                                        .Where(cr => cr.ConsultantId == consultant.Id)
                                                        .OrderByDescending(cr => cr.Id)
                                                        .Select(cr => cr.Record)
                                                        .FirstOrDefault()
                                    }).OrderByDescending(o => o.Id).ToListAsync();

                var mappedData = _mapper.Map<List<ConsultantDto>>(result);
                foreach (var item in mappedData)
                {
                    if (!string.IsNullOrEmpty(item.Resume))
                    {

                        //var resumePath = Path.GetFullPath(Path.Combine(_hostingEnvironment.WebRootPath, item.Resume));

                        //if (resumePath.StartsWith(_hostingEnvironment.WebRootPath) && File.Exists(resumePath))
                        //{
                        //    var extractResult = await ExtractTextGetAsync(resumePath);
                        //    item.ResumeSearchText = extractResult.Data;
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Blocked suspicious path or file not found: {Path}", resumePath);
                        //}
                    }
                }

                return OutputHandler.Handler((int)ResponseType.GET, mappedData, "Consultants", mappedData.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<OutputDTO<ConsultantDto>> SingleConsultantGet(int id)
        {
            try
            {
                var requisitions = await (from a in _context.ConsultantActivities
                                          where a.TenantId == _session.TenantId && a.ConsultantId == id

                                          join client in _context.Clients on a.ClientId equals client.Id into clientJoin
                                          from client in clientJoin.DefaultIfEmpty()

                                          select new ConsultantRequisitionsDto
                                          {
                                              ClientName = client != null ? client.ClientName : null,
                                              RequisitionId = a.RequisitionId,
                                          }).ToListAsync();


                var consultant = await _context.Consultants
                    .Where(x => x.TenantId == _session.TenantId && x.Id == id)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                var mappedCnsultant = _mapper.Map<ConsultantDto>(consultant);
                mappedCnsultant.Requisition = requisitions.ToList();


                return OutputHandler.Handler((int)ResponseType.GET, mappedCnsultant, "Consultant", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<string>> ExtractTextAsync(string fileUrl)
        {
            try
            {
               // string tikaHtmlEndpoint = "http://172.235.58.249:9998/tika/form";
                //string tikaHtmlEndpoint = "http://172.233.152.32:9998/tika/form";
                string tikaHtmlEndpoint = "http://172.233.152.32:8080/tika/form";
                //

                Uri uri = new Uri(fileUrl);
                string relativePath = Uri.UnescapeDataString(uri.AbsolutePath).TrimStart('/');

                // Combine and resolve to full path inside wwwroot
                string fullPath = Path.GetFullPath(Path.Combine(_hostingEnvironment.WebRootPath, relativePath));
                byte[] fileBytes = await File.ReadAllBytesAsync(fullPath);


                // Step 2: Create multipart/form-data content
                using var form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Use a valid filename in the form data
                form.Add(fileContent, "file", Path.GetFileName(fileUrl));

                // Step 3: Set Accept header to request HTML
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

                // Step 4: POST to Tika /tika/form to extract HTML
                var response = await _httpClient.PostAsync(tikaHtmlEndpoint, form);
                response.EnsureSuccessStatusCode();

                // Step 5: Get HTML output
                string html = await response.Content.ReadAsStringAsync();
                return OutputHandler.Handler((int)ResponseType.GET, html, "Consultant", 0);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error extracting HTML content from Tika", ex);
            }
        }



        private async Task<OutputDTO<string>> ExtractTextGetAsync(string fullPath)
        {
            try
            {
                //string tikaHtmlEndpoint = "http://172.235.58.249:9998/tika/form";
                //string tikaHtmlEndpoint = "http://172.233.152.32:9998/tika/form";
                string tikaHtmlEndpoint = "http://172.233.152.32:8080/tika/form";

                //Uri uri = new Uri(fileUrl);
                //string relativePath = Uri.UnescapeDataString(uri.AbsolutePath).TrimStart('/');

                //// Combine and resolve to full path inside wwwroot
                //string fullPath = Path.GetFullPath(Path.Combine(_hostingEnvironment.WebRootPath, relativePath));
                byte[] fileBytes = await File.ReadAllBytesAsync(fullPath);


                // Step 2: Create multipart/form-data content
                using var form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Use a valid filename in the form data
                form.Add(fileContent, "file", Path.GetFileName(fullPath));

                // Step 3: Set Accept header to request HTML
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

                // Step 4: POST to Tika /tika/form to extract HTML
                var response = await _httpClient.PostAsync(tikaHtmlEndpoint, form);
                response.EnsureSuccessStatusCode();

                // Step 5: Get HTML output
                string html = await response.Content.ReadAsStringAsync();
                return OutputHandler.Handler((int)ResponseType.GET, html, "Consultant", 0);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error extracting HTML content from Tika", ex);
            }
        }


        public async Task<OutputDTO<bool>> DeleteConsultantFileAsync(int id)
        {
            try
            {
                var consultant = await _context.Consultants
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (consultant == null)
                {
                    return OutputHandler.Handler<bool>((int)ResponseType.NOT_EXIST, false, "Consultant not found");
                }

                if (string.IsNullOrWhiteSpace(consultant.Resume))
                {
                    return OutputHandler.Handler<bool>((int)ResponseType.NOT_EXIST, false, "No resume found to delete");
                }

                // Construct the full path
                var resumeFullPath = Path.Combine(
                    _hostingEnvironment.WebRootPath,
                    consultant.Resume.Replace("/", Path.DirectorySeparatorChar.ToString())
                );

                if (System.IO.File.Exists(resumeFullPath))
                {
                    System.IO.File.Delete(resumeFullPath);
                }

                // Clear resume path in DB
                consultant.Resume = null;
                _context.Consultants.Update(consultant);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Resume file deleted successfully");
            }
            catch (Exception ex)
            {
                return OutputHandler.Handler<bool>((int)ResponseType.SESSION_EXIST, false, $"An error occurred: {ex.Message}");
            }
        }


        #region Consultant Activity

        public async Task<OutputDTO<List<ConsultantActivityDto>>> SearchConsultantsForActivity(string name)
        {
            try
            {
                var nameLower = name.ToLower();

                var rawData = await (
         from consultant in _context.Consultants
         join call in _context.CallRecords
             on consultant.Id equals call.ConsultantId into callGroup
         from call in callGroup.DefaultIfEmpty()
         where (
            EF.Functions.Like(((consultant.FirstName ?? "") + " " + (consultant.MiddleName ?? "") + " " + (consultant.LastName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.FirstName ?? "") + " " + (consultant.LastName ?? "") + " " + (consultant.MiddleName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.MiddleName ?? "") + " " + (consultant.FirstName ?? "") + " " + (consultant.LastName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.MiddleName ?? "") + " " + (consultant.LastName ?? "") + " " + (consultant.FirstName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.LastName ?? "") + " " + (consultant.FirstName ?? "") + " " + (consultant.MiddleName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.LastName ?? "") + " " + (consultant.MiddleName ?? "") + " " + (consultant.FirstName ?? "")).ToLower(), $"%{nameLower}%") ||

            EF.Functions.Like(((consultant.FirstName ?? "") + (consultant.MiddleName ?? "") + (consultant.LastName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.FirstName ?? "") + (consultant.LastName ?? "") + (consultant.MiddleName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.MiddleName ?? "") + (consultant.FirstName ?? "") + (consultant.LastName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.MiddleName ?? "") + (consultant.LastName ?? "") + (consultant.FirstName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.LastName ?? "") + (consultant.FirstName ?? "") + (consultant.MiddleName ?? "")).ToLower(), $"%{nameLower}%") ||
            EF.Functions.Like(((consultant.LastName ?? "") + (consultant.MiddleName ?? "") + (consultant.FirstName ?? "")).ToLower(), $"%{nameLower}%")

         ) &&
         consultant.TenantId == _session.TenantId
         select new
         {
             ConsultantId = consultant.Id,
             ConsultantName = ((consultant.FirstName ?? "") + " " + (consultant.LastName ?? "")).Trim(),
             AssignedToId = consultant.AssignedToId,
             Call = call
         }
     ).ToListAsync();


                var grouped = rawData
                    .GroupBy(x => new { x.ConsultantId, x.AssignedToId, x.ConsultantName })
                    .Select(g =>
                    {
                        var lastCall = g
                            .Where(x => x.Call != null)
                            .OrderByDescending(x => x.Call.Date)
                            .Select(x => x.Call)
                            .FirstOrDefault();

                        return new ConsultantActivityDto
                        {
                            Id = g.Key.ConsultantId,
                            ConsultantId = g.Key.ConsultantId,
                            ConsultantName = g.Key.ConsultantName,
                            AssignedToId = g.Key.AssignedToId ?? string.Empty,
                            ManagerId = lastCall?.ManagerId ?? 0,
                            ClientId = lastCall?.ClientId ?? 0,
                            LastContact = lastCall?.Date,
                            RequisitionId = 0,
                            BillRate = 0,
                            PayRate = 0,

                        };
                    })
                    .ToList();

                var mapper = _mapper.Map<List<ConsultantActivityDto>>(grouped);

                return OutputHandler.Handler((int)ResponseType.GET, mapper, "Activity", mapper.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<ConsultantActivityDto>> ConsultantActivityAddUpdate(ConsultantActivityDto input)
        {
            try
            {
                var existingActivity = await _context.ConsultantActivities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id);

                var mappedConsultant = _mapper.Map<ConsultantActivity>(input);
                mappedConsultant.TenantId = _session.TenantId;

                ConsultantActivity savedActivity;

                if (existingActivity != null)
                {

                    var updatedEntity = _mapper.Map(input, existingActivity);

                    // Update existing activity
                    _context.ConsultantActivities.Update(updatedEntity);
                    await _context.SaveChangesAsync();
                    savedActivity = updatedEntity;
                }
                else
                {
                    // Add new activity
                    _context.ConsultantActivities.Add(mappedConsultant);
                    await _context.SaveChangesAsync();
                    savedActivity = mappedConsultant;
                }

                var resultDto = _mapper.Map<ConsultantActivityDto>(savedActivity);


                return OutputHandler.Handler((int)(existingActivity != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, "ConsultantActivity");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<bool>> ConsultantActivityDelete(int id)
        {
            try
            {
                var activity = await _context.ConsultantActivities.FirstOrDefaultAsync(x => x.Id == id);
                if (activity == null)
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "ConsultantActivity");

                _context.ConsultantActivities.Remove(activity);
                await _context.SaveChangesAsync();

                var process = await _context.ConsultantInterviewProcesses.FirstOrDefaultAsync(x => x.ConsultantActivityId == id);
                if (activity == null)
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Interview Process");

                _context.ConsultantInterviewProcesses.Remove(process);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "ConsultantActivity");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<ConsultantActivityDto>>> InterviewProcessConsultantsList(int? requisitionId)
        {
            try
            {

                if (requisitionId > 0)
                {
                    var resultByReqId = await (from ca in _context.ConsultantActivities
                                               join c in _context.Consultants on ca.ConsultantId equals c.Id
                                               where ca.RequisitionId == requisitionId
                                               select new ConsultantActivityDto
                                               {
                                                   Id = ca.Id,
                                                   ClientId = ca.ClientId,
                                                   ManagerId = ca.ManagerId,
                                                   ConsultantId = ca.ConsultantId,
                                                   ConsultantName = c.LastName + ", " + c.FirstName,
                                                   RequisitionId = ca.RequisitionId,
                                                   AssignedToId = ca.AssignedToId,
                                                   BillRate = ca.BillRate,
                                                   PayRate = ca.PayRate,
                                                   CreationTime = ca.CreationTime,

                                                   InterviewDateTime = _context.ConsultantInterviewProcesses
                                                   .Where(x => x.ConsultantActivityId == ca.Id)
                                                   .OrderBy(o => o.Id)
                                                   .Select(s => s.Date)
                                                   .ToList(),

                                                   InterviewStatus = _context.ConsultantInterviewProcesses
                                                    .Where(x => x.ConsultantActivityId == ca.Id)
                                                    .OrderBy(o => o.Id)
                                                    .Select(s =>
                                                        ((s.Salary > 0 || s.HourlyRate > 0) ? "Selected"
                                                                : "Interview"))
                                                    .ToList()
                                               }).ToListAsync();

                    resultByReqId.ForEach(item =>
                    {
                        if (item.InterviewStatus == null || item.InterviewStatus.Count == 0)
                        {
                            item.InterviewStatus = new List<string> { "Submitted" };
                        }
                    });

                    return OutputHandler.Handler((int)ResponseType.GET, resultByReqId, "Consultants", resultByReqId.Count);

                }


                var result = await (from ca in _context.ConsultantActivities
                                    join c in _context.Consultants on ca.ConsultantId equals c.Id
                                    select new ConsultantActivityDto
                                    {
                                        Id = ca.Id,
                                        ClientId = ca.ClientId,
                                        ManagerId = ca.ManagerId,
                                        ConsultantId = ca.ConsultantId,
                                        ConsultantName = c.LastName + ", " + c.FirstName,
                                        RequisitionId = ca.RequisitionId,
                                        AssignedToId = ca.AssignedToId,
                                        BillRate = ca.BillRate,
                                        PayRate = ca.PayRate,
                                        CreationTime = ca.CreationTime,

                                        InterviewDateTime = _context.ConsultantInterviewProcesses
                                                   .Where(x => x.ConsultantActivityId == ca.Id)
                                                   .OrderBy(o => o.Id)
                                                   .Select(s => s.Date)
                                                   .ToList(),

                                        InterviewStatus = _context.ConsultantInterviewProcesses
                                                    .Where(x => x.ConsultantActivityId == ca.Id)
                                                    .OrderBy(o => o.Id)
                                                    .Select(s =>
                                                        ((s.Salary > 0 || s.HourlyRate > 0) ? "Selected"
                                                                : "Interview"))
                                                    .ToList()
                                    }).ToListAsync();

                result.ForEach(item =>
                {
                    if (item.InterviewStatus == null || item.InterviewStatus.Count == 0)
                    {
                        item.InterviewStatus = new List<string> { "Submitted" };
                    }
                });

                return OutputHandler.Handler((int)ResponseType.GET, result, "Consultants", result.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<ConsultantActivityDto>> InterviewProcessConsultantSingle(int id)
        {
            try
            {
                var result = await (from ca in _context.ConsultantActivities
                                    join c in _context.Consultants on ca.ConsultantId equals c.Id
                                    where ca.Id == id
                                    select new ConsultantActivityDto
                                    {
                                        Id = ca.Id,
                                        ClientId = ca.ClientId,
                                        ManagerId = ca.ManagerId,
                                        ConsultantId = ca.ConsultantId,
                                        ConsultantName = c.LastName + " " + c.FirstName,
                                        RequisitionId = ca.RequisitionId,
                                        AssignedToId = ca.AssignedToId,
                                        BillRate = ca.BillRate,
                                        PayRate = ca.PayRate,
                                        CreationTime = ca.CreationTime,

                                        InterviewDateTime = _context.ConsultantInterviewProcesses
                                                   .Where(x => x.ConsultantActivityId == ca.Id)
                                                   .OrderBy(o => o.Id)
                                                   .Select(s => s.Date)
                                                   .ToList(),

                                        InterviewStatus = _context.ConsultantInterviewProcesses
                                                    .Where(x => x.ConsultantActivityId == ca.Id)
                                                    .OrderBy(o => o.Id)
                                                    .Select(s =>
                                                        ((s.Salary > 0 || s.HourlyRate > 0) ? "Selected"
                                                                : "Interview"))
                                                    .ToList()
                                    }).FirstOrDefaultAsync();

                    if (result.InterviewStatus == null || result.InterviewStatus.Count == 0)
                    {
                        result.InterviewStatus = new List<string> { "Submitted" };
                    }

                return OutputHandler.Handler((int)ResponseType.GET, result, "Consultant", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion



        #region Consultant Interview Process



        public async Task<OutputDTO<ConsultantInterviewProcessDto>> InterviewProcessAddUpdate(ConsultantInterviewProcessDto input)
        {
            try
            {
                var existing = await _context.ConsultantInterviewProcesses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id);

                var mappedConsultant = _mapper.Map<ConsultantInterviewProcess>(input);
                mappedConsultant.TenantId = _session.TenantId;

                ConsultantInterviewProcess savedActivity;

                if (existing != null)
                {

                    var updatedEntity = _mapper.Map(input, existing);

                    // Update existing activity
                    _context.ConsultantInterviewProcesses.Update(updatedEntity);
                    await _context.SaveChangesAsync();
                    savedActivity = updatedEntity;
                }
                else
                {
                    // Add new activity
                    _context.ConsultantInterviewProcesses.Add(mappedConsultant);
                    await _context.SaveChangesAsync();
                    savedActivity = mappedConsultant;
                }

                var resultDto = _mapper.Map<ConsultantInterviewProcessDto>(savedActivity);


                return OutputHandler.Handler((int)(existing != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, "Interview Process");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<bool>> InterviewProcessDelete(int id)
        {
            try
            {
                var activity = await _context.ConsultantInterviewProcesses.FirstOrDefaultAsync(x => x.Id == id);
                if (activity == null)
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Interview Process");

                _context.ConsultantInterviewProcesses.Remove(activity);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Interview Process");
            }
            catch (Exception)
            {
                throw;
            }
        }



        //public async Task<OutputDTO<List<ConsultantInterviewProcessDto>>> InterviewProcessesList(int? requisitionId)
        //{
        //    try
        //    {

        //        if (requisitionId > 0)
        //        {
        //            var resultByReqId = await (from ca in _context.ConsultantActivities
        //                                       join c in _context.Consultants on ca.ConsultantId equals c.Id
        //                                       where ca.RequisitionId == requisitionId
        //                                       select new ConsultantInterviewProcessDto
        //                                       {
        //                                           Id = ca.Id,
        //                                           ClientId = ca.ClientId,
        //                                           ManagerId = ca.ManagerId,
        //                                           ConsultantId = ca.ConsultantId,
        //                                           ConsultantName = c.LastName + " " + c.FirstName,
        //                                           RequisitionId = ca.RequisitionId,
        //                                           AssignedToId = ca.AssignedToId,
        //                                           BillRate = ca.BillRate,
        //                                           PayRate = ca.PayRate,
        //                                           CreationTime = ca.CreationTime,
        //                                       }).ToListAsync();

        //            return OutputHandler.Handler((int)ResponseType.GET, resultByReqId, "Interview Process", resultByReqId.Count);

        //        }


        //        var result = await (from ca in _context.ConsultantActivities
        //                            join c in _context.Consultants on ca.ConsultantId equals c.Id
        //                            select new ConsultantInterviewProcessDto
        //                            {
        //                                Id = ca.Id,
        //                                ClientId = ca.ClientId,
        //                                ManagerId = ca.ManagerId,
        //                                ConsultantId = ca.ConsultantId,
        //                                ConsultantName = c.LastName + " " + c.FirstName,
        //                                RequisitionId = ca.RequisitionId,
        //                                AssignedToId = ca.AssignedToId,
        //                                BillRate = ca.BillRate,
        //                                PayRate = ca.PayRate,
        //                                CreationTime = ca.CreationTime,
        //                            }).ToListAsync();

        //        return OutputHandler.Handler((int)ResponseType.GET, result, "Interview Process", result.Count);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}



        //public async Task<OutputDTO<ConsultantInterviewProcessDto>> InterviewProcessesSingle(int id)
        //{
        //    try
        //    {
        //        var result = await (from ca in _context.ConsultantActivities
        //                            join c in _context.Consultants on ca.ConsultantId equals c.Id
        //                            where ca.Id == id
        //                            select new ConsultantInterviewProcessDto
        //                            {
        //                                Id = ca.Id,
        //                                ClientId = ca.ClientId,
        //                                ManagerId = ca.ManagerId,
        //                                ConsultantId = ca.ConsultantId,
        //                                ConsultantName = c.LastName + " " + c.FirstName,
        //                                RequisitionId = ca.RequisitionId,
        //                                AssignedToId = ca.AssignedToId,
        //                                BillRate = ca.BillRate,
        //                                PayRate = ca.PayRate,
        //                                CreationTime = ca.CreationTime,
        //                            }).FirstOrDefaultAsync();

        //        return OutputHandler.Handler((int)ResponseType.GET, result, "Interview Process", 0);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}



        #endregion

        #region Interview

        public async Task<OutputDTO<bool>> InterviewAddUpdate(ConsultantInterviewDto input)
        {
            try
            {
                var existing = await _context.ConsultantInterviews
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mapped = _mapper.Map<ConsultantInterview>(input);
                mapped.TenantId = _session.TenantId;

                if (existing != null)
                {
                    var updated = _mapper.Map(input, existing);
                    _context.ConsultantInterviews.Update(updated);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.UPDATE, true, "Consultant Interview");
                }
                else
                {
                    _context.ConsultantInterviews.Add(mapped);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.CREATE, true, "Consultant Interview");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<bool>> InterviewDelete(int id)
        {
            try
            {
                var item = await _context.ConsultantInterviews
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (item == null)
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Consultant Interview");

                _context.ConsultantInterviews.Remove(item);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Consultant Interview");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<ConsultantInterviewDto>>> InterviewListGet()
        {
            try
            {
                var list = await _context.ConsultantInterviews
                    .Where(x => x.TenantId == _session.TenantId)
                    .ToListAsync();

                var mapped = _mapper.Map<List<ConsultantInterviewDto>>(list);
                return OutputHandler.Handler((int)ResponseType.GET, mapped, "Consultant Interviews", mapped.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region Reference
        public async Task<OutputDTO<bool>> ReferenceAddUpdate(ConsultantReferenceDto input)
        {
            try
            {
                var existing = await _context.ConsultantReferences
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mapped = _mapper.Map<ConsultantReference>(input);
                mapped.TenantId = _session.TenantId;

                if (existing != null)
                {
                    var updated = _mapper.Map(input, existing);
                    _context.ConsultantReferences.Update(updated);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.UPDATE, true, "Consultant Reference");
                }
                else
                {
                    _context.ConsultantReferences.Add(mapped);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.CREATE, true, "Consultant Reference");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<bool>> ReferenceDelete(int id)
        {
            try
            {
                var item = await _context.ConsultantReferences
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (item == null)
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Consultant Reference");

                _context.ConsultantReferences.Remove(item);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Consultant Reference");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<ConsultantReferenceDto>>> ReferenceListGet()
        {
            try
            {
                var list = await _context.ConsultantReferences
                    .Where(x => x.TenantId == _session.TenantId)
                    .ToListAsync();

                var mapped = _mapper.Map<List<ConsultantReferenceDto>>(list);
                return OutputHandler.Handler((int)ResponseType.GET, mapped, "Consultant References", mapped.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
