using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vega_API.Core.Models;
using Vega_API.Models;

namespace Vega_API.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplayOrdering<T>(this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnMap)
        {

            if (String.IsNullOrWhiteSpace(queryObj.SortBy) || !columnMap.ContainsKey(queryObj.SortBy))
                return query;

            if (queryObj.IsSortAscending)
                return query = query.OrderBy(columnMap[queryObj.SortBy]);
            else
                return query = query.OrderByDescending(columnMap[queryObj.SortBy]);
        }

        public static IQueryable<T> ApplayPaging<T>(this IQueryable<T> query, IQueryObject queryObj)
        {
            if (queryObj.Page <= 0)
                queryObj.Page = 1;
            if (queryObj.PageSize <= 0)
                queryObj.PageSize = 10;

            return query.Skip((queryObj.Page - 1) * queryObj.PageSize).Take(queryObj.PageSize);

        }

    }
}
