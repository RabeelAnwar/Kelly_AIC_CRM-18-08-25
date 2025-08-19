using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Utility.DTOs.Pagination
{
    public static class PaginationMethod
    {
        public static async Task<PaginatedListDTO<T>> ToPaggedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedListDTO<T>(items, count, pageNumber, pageSize);
        }
    }
}
