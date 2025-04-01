using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync( )
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<PagedList<T>> GetPagedAsync(PaginationParams parameters)
    {
        var query = _dbSet.AsQueryable();
        return await PagedList<T>.CreateAsync(query , parameters.PageNumber , parameters.PageSize);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T , bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T , bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<T?> LastOrDefaultAsync(Expression<Func<T , bool>> predicate)
    {
        return await _dbSet.LastOrDefaultAsync(predicate);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T , bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T , bool>>? predicate = null)
    {
        if (predicate == null)
            return await _dbSet.CountAsync();
        return await _dbSet.CountAsync(predicate);
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }

    public virtual IQueryable<T> GetQueryable( )
    {
        return _dbSet;
    }

    public virtual async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T , object>>[] includes)
    {
        var query = _dbSet.AsQueryable();
        query = includes.Aggregate(query , (current , include) => current.Include(include));
        return await query.ToListAsync();
    }

    public virtual async Task<PagedList<T>> GetPagedWithIncludesAsync(PaginationParams parameters , params Expression<Func<T , object>>[] includes)
    {
        var query = _dbSet.AsQueryable();
        query = includes.Aggregate(query , (current , include) => current.Include(include));
        return await PagedList<T>.CreateAsync(query , parameters.PageNumber , parameters.PageSize);
    }

    public virtual async Task<int> SaveChangesAsync( )
    {
        return await _context.SaveChangesAsync();
    }

    Task<IEnumerable<T>> IRepository<T>.AddRangeAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    Task IRepository<T>.DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    Task IRepository<T>.DeleteRangeAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    Task<bool> IRepository<T>.ExistsAsync(Expression<Func<T , bool>> predicate)
    {
        throw new NotImplementedException();
    }
}
