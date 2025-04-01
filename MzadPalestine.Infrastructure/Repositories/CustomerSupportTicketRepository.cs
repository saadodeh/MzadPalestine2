using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;
using System.Linq.Expressions;

namespace MzadPalestine.Infrastructure.Repositories;

public class CustomerSupportTicketRepository : GenericRepository<CustomerSupportTicket>, ICustomerSupportTicketRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerSupportTicketRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<CustomerSupportTicket?> GetByIdAsync(int id)
    {
        return await _context.Set<CustomerSupportTicket>()
            .Include(t => t.User)
            .Include(t => t.Admin)
            .Include(t => t.Messages)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public override async Task<IEnumerable<CustomerSupportTicket>> GetAllAsync()
    {
        return await _context.Set<CustomerSupportTicket>()
            .Include(t => t.User)
            .Include(t => t.Messages)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<CustomerSupportTicket>> GetUserTicketsAsync(string userId)
    {
        return await _context.Set<CustomerSupportTicket>()
            .Include(t => t.Messages)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<CustomerSupportTicket>> GetTicketsByStatusAsync(TicketStatus status)
    {
        return await _context.Set<CustomerSupportTicket>()
            .Include(t => t.User)
            .Include(t => t.Messages)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public override async Task<CustomerSupportTicket> AddAsync(CustomerSupportTicket entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.Status = TicketStatus.Open;
        await _context.Set<CustomerSupportTicket>().AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<CustomerSupportTicket>> AddRangeAsync(IEnumerable<CustomerSupportTicket> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.Status = TicketStatus.Open;
        }
        await _context.Set<CustomerSupportTicket>().AddRangeAsync(entities);
        return entities;
    }

    public async Task DeleteAsync(CustomerSupportTicket entity)
    {
        _context.Set<CustomerSupportTicket>().Remove(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<CustomerSupportTicket> entities)
    {
        _context.Set<CustomerSupportTicket>().RemoveRange(entities);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Expression<Func<CustomerSupportTicket, bool>> predicate)
    {
        return await _context.Set<CustomerSupportTicket>().AnyAsync(predicate);
    }
}