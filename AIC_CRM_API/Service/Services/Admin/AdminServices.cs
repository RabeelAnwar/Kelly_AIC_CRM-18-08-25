
using DataAccess.Entities;
using DeparmentEntity = DataAccess.Entities.Department;
using DocumentTypeEntity = DataAccess.Entities.DocumentType;
using CallTypeEntity = DataAccess.Entities.CallType;
using ContactTypeEntity = DataAccess.Entities.ContactType;
using SkillMasterEntity = DataAccess.Entities.SkillMaster;
using SupportTicketEntity = DataAccess.Entities.SupportTicket;

using Microsoft.EntityFrameworkCore;
using Service.Services.Admin.Department;
using Service.Sessions;
using Utility.OutputData;
using Service.Services.Admin.DocumentType;
using Service.Services.Admin.CallType;
using Service.Services.Admin.ContactType;
using Service.Services.Admin.SkillMaster;
using Microsoft.AspNetCore.Identity;
using Service.Converter;
using Service.Services.Consultant.DTOs;
using Service.Services.Admin.SupportTicket;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Service.FileUpload;

namespace Service.Services.Admin
{
    public class AdminServices : IAdminServices
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IFileUpload _fileUpload;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminServices(MainContext context,
            IMapper mapper,
            ISessionData sessionData,
            RoleManager<IdentityRole> roleManager,
            IFileUpload file,
            IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _session = sessionData;
            _roleManager = roleManager;
            _fileUpload = file;
            _hostingEnvironment = hostEnvironment;

        }


        #region Department

        public async Task<OutputDTO<bool>> DepartmentAddUpdate(DepartmentDto input)
        {
            try
            {
                var department = await _context.Departments
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedInput = _mapper.Map<DeparmentEntity>(input);

                if (department != null)
                {
                    department.Name = input.Name;
                    department.IsActive = input.IsActive;
                    _context.Departments.Update(department);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.UPDATE, true, "Department");
                }
                else
                {
                    _context.Departments.Add(mappedInput);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.CREATE, true, "Department");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<bool>> DepartmentDelete(int id)
        {
            try
            {
                var department = await _context.Departments
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (department != null)
                {
                    _context.Departments.Remove(department);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.DELETE, true, "Department");
                }

                return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Department");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<DepartmentDto>>> DepartmentGet()
        {
            try
            {
                var departments = await _context.Departments
                    .Where(d => d.TenantId == _session.TenantId)
                    .Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        IsActive = d.IsActive
                    })
                    .ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, departments, "Department", departments.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion


        #region Document Type

        public async Task<OutputDTO<bool>> DocumentTypeAddUpdate(DocumentTypeDto input)
        {
            try
            {
                var documentType = await _context.DocumentTypes
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedInput = _mapper.Map<DocumentTypeEntity>(input);

                if (documentType != null)
                {
                    var mapper = _mapper.Map(input, documentType);
                    _context.DocumentTypes.Update(mapper);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.UPDATE, true, "DocumentType");
                }
                else
                {
                    _context.DocumentTypes.Add(mappedInput);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.CREATE, true, "DocumentType");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<bool>> DocumentTypeDelete(int id)
        {
            try
            {
                var documentType = await _context.DocumentTypes
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (documentType != null)
                {
                    _context.DocumentTypes.Remove(documentType);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.DELETE, true, "DocumentType");
                }

                return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "DocumentType");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<List<DocumentTypeDto>>> DocumentTypeGet()
        {
            try
            {
                var documentTypes = await _context.DocumentTypes
                    .Where(d => d.TenantId == _session.TenantId)
                    .Select(d => new DocumentTypeDto
                    {
                        Id = d.Id,
                        DocumentTypeName = d.DocumentTypeName,
                        UserTypeId = d.UserTypeId
                    })
                    .ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, documentTypes, "DocumentType", documentTypes.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion


        #region Call Type

        public async Task<OutputDTO<bool>> CallTypeAddUpdate(CallTypeDto input)
        {
            try
            {
                var callType = await _context.CallTypes
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedInput = _mapper.Map<CallTypeEntity>(input);

                if (callType != null)
                {
                    callType.Name = input.Name;
                    callType.IsActive = input.IsActive;
                    _context.CallTypes.Update(callType);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.UPDATE, true, "CallType");
                }
                else
                {
                    _context.CallTypes.Add(mappedInput);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.CREATE, true, "CallType");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<bool>> CallTypeDelete(int id)
        {
            try
            {
                var callType = await _context.CallTypes
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (callType != null)
                {
                    _context.CallTypes.Remove(callType);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.DELETE, true, "CallType");
                }

                return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "CallType");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<List<CallTypeDto>>> CallTypeGet()
        {
            try
            {
                var callTypes = await _context.CallTypes
                    .Where(ct => ct.TenantId == _session.TenantId)
                    .Select(ct => new CallTypeDto
                    {
                        Id = ct.Id,
                        Name = ct.Name,
                        IsActive = ct.IsActive
                    })
                    .ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, callTypes, "CallType", callTypes.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion


        #region Contact Type

        public async Task<OutputDTO<bool>> ContactTypeAddUpdate(ContactTypeDto input)
        {
            try
            {
                var contactType = await _context.ContactTypes
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedInput = _mapper.Map<ContactTypeEntity>(input);

                if (contactType != null)
                {
                    contactType.Name = input.Name;
                    _context.ContactTypes.Update(contactType);
                    var result = await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.UPDATE, true, "ContactType");
                }
                else
                {
                    _context.ContactTypes.Add(mappedInput);
                    await _context.SaveChangesAsync();
                    var result = await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.CREATE, true, "ContactType");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<bool>> ContactTypeDelete(int id)
        {
            try
            {
                var contactType = await _context.ContactTypes
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (contactType != null)
                {
                    _context.ContactTypes.Remove(contactType);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.DELETE, true, "ContactType");
                }

                return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "ContactType");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<List<ContactTypeDto>>> ContactTypeGet()
        {
            try
            {
                var contactTypes = await _context.ContactTypes
                    .Where(ct => ct.TenantId == _session.TenantId)
                    .Select(ct => new ContactTypeDto
                    {
                        Id = ct.Id,
                        Name = ct.Name
                    })
                    .ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, contactTypes, "ContactType", contactTypes.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<OutputDTO<bool>> ContactTypeAddUpdate(ContactTypeDto input)
        //{
        //    try
        //    {
        //        // Validate input
        //        if (string.IsNullOrWhiteSpace(input.Name))
        //            return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, "Contact type name is required");

        //        var contactType = await _context.ContactTypes
        //            .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

        //        if (contactType != null)
        //        {
        //            // Update existing contact type
        //            contactType.Name = input.Name;
        //            contactType.LastModificationTime = DateTime.UtcNow;
        //            contactType.LastModifierUserId = _session.UserId;
        //            _context.ContactTypes.Update(contactType);

        //            // Update corresponding Identity role
        //            var role = await _roleManager.FindByNameAsync(contactType.Name);
        //            if (role != null)
        //            {
        //                role.Name = input.Name;
        //                var roleResult = await _roleManager.UpdateAsync(role);
        //                if (!roleResult.Succeeded)
        //                {
        //                    var error = roleResult.Errors.FirstOrDefault()?.Description ?? "Failed to update role";
        //                    return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, error);
        //                }
        //            }
        //            else
        //            {
        //                // Create role if it doesn't exist
        //                var roleResult = await _roleManager.CreateAsync(new IdentityRole(input.Name));
        //                if (!roleResult.Succeeded)
        //                {
        //                    var error = roleResult.Errors.FirstOrDefault()?.Description ?? "Failed to create role";
        //                    return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, error);
        //                }
        //            }

        //            await _context.SaveChangesAsync();
        //            return OutputHandler.Handler((int)ResponseType.UPDATE, true, entity);
        //        }
        //        else
        //        {
        //            // Create new contact type
        //            var newContactType = _mapper.Map<ContactTypeEntity>(input);
        //            newContactType.TenantId = _session.TenantId;
        //            newContactType.CreationTime = DateTime.UtcNow;
        //            newContactType.CreatorUserId = _session.UserId;
        //            _context.ContactTypes.Add(newContactType);

        //            // Create corresponding Identity role
        //            var roleResult = await _roleManager.CreateAsync(new IdentityRole(input.Name));
        //            if (!roleResult.Succeeded)
        //            {
        //                var error = roleResult.Errors.FirstOrDefault()?.Description ?? "Failed to create role";
        //                return OutputHandler.Handler((-Utility.OutputData.ResponseType.INVALID_REQUEST, false, error);
        //            }

        //            await _context.SaveChangesAsync();
        //            return OutputHandler.Handler((int)ResponseType.CREATE, true, entity);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, ex.Message);
        //    }
        //}

        //public async Task<OutputDTO<bool>> ContactTypeDelete(int id)
        //{
        //    try
        //    {
        //        var contactType = await _context.ContactTypes
        //            .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

        //        if (contactType == null)
        //            return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, entity);

        //        // Check if contact type is associated with users
        //        var usersWithRole = await _context.Users
        //            .AnyAsync(u => u.ContactTypeId == id && u.TenantId == _session.TenantId && !u.IsDeleted);
        //        if (usersWithRole)
        //            return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, "Cannot delete contact type assigned to users");

        //        // Delete corresponding Identity role
        //        var role = await _roleManager.FindByNameAsync(contactType.Name);
        //        if (role != null)
        //        {
        //            var roleResult = await _roleManager.DeleteAsync(role);
        //            if (!roleResult.Succeeded)
        //            {
        //                var error = roleResult.Errors.FirstOrDefault()?.Description ?? "Failed to delete role";
        //                return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, error);
        //            }
        //        }

        //        _context.ContactTypes.Remove(contactType);
        //        await _context.SaveChangesAsync();
        //        return OutputHandler.Handler((int)ResponseType.DELETE, true, entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        return OutputHandler.Handler((int)ResponseType.INVALID_REQUEST, false, ex.Message);
        //    }
        //}

        //public async Task<OutputDTO<List<ContactTypeDto>>> ContactTypeGet()
        //{
        //    try
        //    {
        //        var contactTypes = await _context.ContactTypes
        //            .Where(ct => ct.TenantId == _session.TenantId && !ct.IsDeleted)
        //            .Select(ct => _mapper.Map<ContactTypeDto>(ct))
        //            .ToListAsync();

        //        // Optionally, sync with Identity roles
        //        var identityRoles = await _roleManager.Roles
        //            .Where(r => contactTypes.Any(ct => ct.Name == r.Name))
        //            .Select(r => r.Name)
        //            .ToListAsync();

        //        // Ensure all contact types have corresponding roles
        //        foreach (var ct in contactTypes)
        //        {
        //            if (!identityRoles.Contains(ct.Name) && !await _roleManager.RoleExistsAsync(ct.Name))
        //            {
        //                await _roleManager.CreateAsync(new IdentityRole(ct.Name));
        //            }
        //        }

        //        return OutputHandler.Handler((int)ResponseType.GET, contactTypes, entity, contactTypes.Count);
        //    }
        //    catch (Exception ex)
        //    {
        //        return OutputHandler.Handler<List<ContactTypeDto>>((int)ResponseType.INVALID_REQUEST, null, ex.Message, 0);
        //    }
        //}

        #endregion


        #region Skill Master

        public async Task<OutputDTO<bool>> SkillMasterAddUpdate(SkillMasterDto input)
        {
            try
            {
                var skill = await _context.SkillMasters
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedInput = _mapper.Map<SkillMasterEntity>(input);

                if (skill != null)
                {
                    skill.Name = input.Name;
                    _context.SkillMasters.Update(skill);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.UPDATE, true, "SkillMaster");
                }
                else
                {
                    _context.SkillMasters.Add(mappedInput);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.CREATE, true, "SkillMaster");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<bool>> SkillMasterDelete(int id)
        {
            try
            {
                var skill = await _context.SkillMasters
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (skill != null)
                {
                    _context.SkillMasters.Remove(skill);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.DELETE, true, "SkillMaster");
                }

                return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "SkillMaster");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<List<SkillMasterDto>>> SkillMasterGet()
        {
            try
            {
                var skills = await _context.SkillMasters
                    .Where(s => s.TenantId == _session.TenantId)
                    .Select(s => new SkillMasterDto
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, skills, "SkillMaster", skills.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Support Ticket


        public async Task<OutputDTO<SupportTicketDto>> SupportTicketAddUpdate(SupportTicketDto input)
        {
            try
            {
                var existingTicket = await _context.SupportTickets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedTicket = _mapper.Map<SupportTicketEntity>(input);
                mappedTicket.TenantId = _session.TenantId;


                var resumePath = string.Empty;

                if (input.DocumentFile != null)
                {
                    resumePath = Path.Combine("Tenants", _session.TenantId.ToString(), "Ticket", input.DocumentFile.FileName).Replace("\\", "/");
                    mappedTicket.Document = resumePath;
                }

                SupportTicketEntity savedTicket;

                if (existingTicket != null)
                {
                    var updatedEntity = _mapper.Map(input, existingTicket);
                    updatedEntity.Document = resumePath;
                    updatedEntity.TenantId = _session.TenantId;

                    _context.SupportTickets.Update(updatedEntity);
                    await _context.SaveChangesAsync();
                    savedTicket = updatedEntity;
                }
                else
                {
                    _context.SupportTickets.Add(mappedTicket);
                    await _context.SaveChangesAsync();
                    savedTicket = mappedTicket;
                }

                if (input.DocumentFile != null)
                {
                    // Upload resume file after save
                    _fileUpload.fileUpload(input.DocumentFile, _session.TenantId.ToString(), "Ticket", input.DocumentFile.FileName, _hostingEnvironment);
                }

                var resultDto = _mapper.Map<SupportTicketDto>(savedTicket);
                return OutputHandler.Handler((int)(existingTicket != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, "Ticket");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<bool>> SupportTicketDelete(int id)
        {
            try
            {
                var ticket = await _context.SupportTickets.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);
                if (ticket == null) return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Ticket");

                _context.SupportTickets.Remove(ticket);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Ticket");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<SupportTicketDto>>> SupportTicketListGet()
        {
            try
            {
                var tenantId = _session.TenantId;

                var result = _context.SupportTickets.ToList();

                var mappedData = _mapper.Map<List<SupportTicketDto>>(result);

                return OutputHandler.Handler((int)ResponseType.GET, mappedData, "Ticket", mappedData.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

    }
}
