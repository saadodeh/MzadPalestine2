using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<bool> AddAsync(ApplicationUser entity);
    Task<bool> UpdateAsync(ApplicationUser entity);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
    Task<ApplicationUser?> GetUserWithRolesAsync(string userId);
    Task<PagedList<ApplicationUser>> GetUsersWithRolesAsync(PaginationParams parameters);
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> IsPhoneUniqueAsync(string phone);
    Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string role);
    Task<decimal> GetUserBalanceAsync(string userId);
    Task UpdateUserBalanceAsync(string userId, decimal amount);
}
