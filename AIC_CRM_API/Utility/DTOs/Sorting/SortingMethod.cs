using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utility.DTOs.Sorting
{
    public static class SortingMethod
    {
        public static IQueryable<TEntity> ApplySorting<TEntity>(this IQueryable<TEntity> query, string sortby, string sortDirection = "desc") where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(sortby))
            {
                return query;
            }
            var sortBy = sortby; // ToPascalCase(sortby);
            var entityType = typeof(TEntity);
            var propertyName = sortBy.Trim();
            var property = entityType.GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{entityType.Name}'.");
            }

            var parameter = Expression.Parameter(entityType, "entity");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            if (sortDirection.Trim().ToLower() == "desc")
            {
                var orderByDesc = Expression.Call(
                    typeof(Queryable),
                    "OrderByDescending",
                    new[] { entityType, property.PropertyType },
                    query.Expression,
                    Expression.Quote(orderByExpression)
                );

                return query.Provider.CreateQuery<TEntity>(orderByDesc);
            }
            else
            {
                var orderByAsc = Expression.Call(
                    typeof(Queryable),
                    "OrderBy",
                    new[] { entityType, property.PropertyType },
                    query.Expression,
                    Expression.Quote(orderByExpression)
                );

                return query.Provider.CreateQuery<TEntity>(orderByAsc);
            }
        }
    }
}
