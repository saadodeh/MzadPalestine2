using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<bool> AddAsync(ApplicationUser entity);
    Task<bool> UpdateAsync(ApplicationUser entity);
    Task<bool> DeleteAsync(string id);
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
    Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(string roleName);
    Task<bool> IsInRoleAsync(string userId, string roleName);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm);
    Task<bool> HasWalletAsync(string userId);
    Task<Wallet?> GetUserWalletAsync(string userId);
    Task<decimal> GetUserBalanceAsync(string userId);
}
