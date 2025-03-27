using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    public static IQueryable<T> ApplyPaging<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
    {
        return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public static IQueryable<T> ApplySoftDelete<T>(this IQueryable<T> query) where T : BaseEntity
    {
        return query.Where(e => !e.IsDeleted);
    }
}
