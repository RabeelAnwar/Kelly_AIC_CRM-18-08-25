using System.Security.Claims;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Extensions;
using Service.Services.Auth;
using Service.Services.Company.DTOs;
using Service.Services.User;
using Service.Services.User.UserMaster;
using Service.Sessions;
using Utility.OutputData;
using CompanyEntity = DataAccess.Entities.Company;

namespace Service.Services.Company
{
    public class CompanyService : ICompanyService
    {

        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;
        private readonly IUserMasterService _users;
        private readonly RoleManager<IdentityRole> _roleManager;


        public CompanyService(MainContext context, IMapper mapper, ISessionData sessionData, IUserMasterService user, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _users = user;
            _mapper = mapper;
            _session = sessionData;
            _roleManager = roleManager;
        }

        public async Task<OutputDTO<bool>> CompanyAddUpdate(CompanyInput input)
        {
            try
            {
                var mappedCompany = _mapper.Map<CompanyEntity>(input);

                CompanyEntity? company = _context.Companies.AsNoTracking().Where(x => x.Id == input.Id).FirstOrDefault();

                if (company != null)
                {
                    var mapper = _mapper.Map(input, company);

                    _context.Companies.Update(mapper);
                    await _context.SaveChangesAsync();

                    return OutputHandler.Handler((int)ResponseType.UPDATE, true, "Company");
                }
                else
                {

                    var lastCompany = _context.Companies.OrderBy(c => c.Id).LastOrDefault();

                    if (lastCompany != null)
                    {
                        mappedCompany.TenantId = lastCompany.TenantId + 1;
                    }
                    else
                    {
                        mappedCompany.TenantId = 1001;
                    }

                    _context.Companies.Add(mappedCompany);

                    var resp = await _context.SaveChangesAsync();

                    if (resp > 0)
                    {
                        await CreateAdminUser(mappedCompany.TenantId);
                    }

                    return OutputHandler.Handler((int)ResponseType.CREATE, true, "Company");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<OutputDTO<CompanyInput>> CompanyProfileGet()
        {
            try
            {
                var company = _context.Companies.Where(c => c.TenantId == _session.TenantId).FirstOrDefault();

                var mappedCompany = _mapper.Map<CompanyInput>(company);

                return OutputHandler.Handler((int)ResponseType.GET, mappedCompany, "Company", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<CompanyInput>>> CompaniesListGet()
        {
            try
            {
                var company = await _context.Companies.OrderBy(x => x.Id).ToListAsync();
                var mappedCompany = _mapper.Map<List<CompanyInput>>(company);

                return OutputHandler.Handler((int)ResponseType.GET, mappedCompany, "Company", mappedCompany.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<bool>> CompanyDelete(int id)
        {
            try
            {
                CompanyEntity? company;

                if (_session.TenantId == 1001)
                {
                    company = _context.Companies.Where(x => x.Id == id).FirstOrDefault();
                }
                else
                {
                    company = _context.Companies.Where(x => x.Id == id && x.TenantId == _session.TenantId).FirstOrDefault();
                }

                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Company");

            }
            catch (Exception)
            {

                throw;
            }

        }

        private async Task<OutputDTO<bool>> CreateAdminUser(int tenantId)
        {
            var user = new UserMasterDto
            {
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                ContactTypeName = "Admin",
                ActiveStatus = true,
                State = "N/A",
                City = "N/A",
                ContactTypeId = 1,
                TenantId = tenantId,
                CreatorUserId = _session.UserId,
                Password = "123"
            };

            return await _users.UserAddUpdate(user);
        }


    }
}