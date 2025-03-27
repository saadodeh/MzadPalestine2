using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        var totalCount = await Task.FromResult(query.Count());
        var items = await Task.FromResult(query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());

        return new PagedList<T>(items, totalCount, pageNumber, pageSize);
    }
}
