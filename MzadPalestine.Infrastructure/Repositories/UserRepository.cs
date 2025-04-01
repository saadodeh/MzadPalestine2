using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Infrastructure.Data;

namespace MzadPalestine.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(ApplicationDbContext context , UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> AddAsync(ApplicationUser entity)
    {
        await _context.Users.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<ApplicationUser> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<PagedResult<ApplicationUser>> GetAllAsync(PaginationParams parameters)
    {
        var query = _context.Users
            .Include(u => u.Wallet)
            .OrderByDescending(u => u.CreatedAt);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new PagedResult<ApplicationUser>
        {
            Items = items ,
            TotalCount = totalCount ,
            PageNumber = parameters.PageNumber ,
            PageSize = parameters.PageSize ,
            TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize)
        };
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByIdAsync(string id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        return await _context.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> UpdateAsync(ApplicationUser entity)
    {
        _context.Users.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        return await UpdateAsync(user);
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync( )
    {
        var parameters = new PaginationParams(); // Using default pagination parameters
        var pagedResult = await GetAllAsync(parameters);
        return pagedResult.Items;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await GetByIdAsync(userId);
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
        return await _context.Users
            .Include(u => u.Wallet)
            .Where(u => usersInRole.Contains(u))
            .ToListAsync();
    }

    public async Task<bool> IsInRoleAsync(string userId , string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        return await _userManager.IsInRoleAsync(user , roleName);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Enumerable.Empty<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm)
    {
        return await _context.Users
            .Include(u => u.Wallet)
            .Where(u =>
                u.UserName.Contains(searchTerm) ||
                u.Email.Contains(searchTerm) ||
                u.PhoneNumber.Contains(searchTerm))
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> HasWalletAsync(string userId)
    {
        return await _context.Users
            .Include(u => u.Wallet)
            .AnyAsync(u => u.Id == userId && u.Wallet != null);
    }

    public async Task<Wallet?> GetUserWalletAsync(string userId)
    {
        var user = await _context.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.Wallet;
    }

    public async Task<decimal> GetUserBalanceAsync(string userId)
    {
        var user = await _context.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.Wallet?.Balance ?? 0;
    }
}
