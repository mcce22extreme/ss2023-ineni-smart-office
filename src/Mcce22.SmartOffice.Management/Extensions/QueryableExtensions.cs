using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<T[]> ToArrayAsync<T>(this IQueryable<T> source)
        {
            var result = await source.ToListAsync();

            return result.ToArray();
        }
    }
}
