using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Helper
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParameters<T>(this HttpContext httpContext,
            IQueryable<T> queryable, int recordsPerPage)
            {
                if (httpContext == null)
                    throw new ArgumentNullException(nameof(httpContext));

                double count = await queryable.CountAsync();
                double totalAmountPages = Math.Ceiling(count/recordsPerPage);
                httpContext.Response.Headers.Add("totalAmountPages", totalAmountPages.ToString());
            }
    }
}