using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Auth.DTOs;
using Service.Services.Company.DTOs;
using Utility.OutputData;

namespace Service.Services.Company
{
    public interface ICompanyService
    {
        Task<OutputDTO<bool>> CompanyAddUpdate(CompanyInput company);
        Task<OutputDTO<bool>> CompanyDelete(int id);
        Task<OutputDTO<List<CompanyInput>>> CompaniesListGet();
        Task<OutputDTO<CompanyInput>> CompanyProfileGet();

    }
}
