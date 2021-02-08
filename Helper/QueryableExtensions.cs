using System.Linq;
using MoviesAPI.DTO;

namespace MoviesAPI.Helper
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * pagination.RecordPerPage)
                .Take(pagination.RecordPerPage);
        }
    }
}