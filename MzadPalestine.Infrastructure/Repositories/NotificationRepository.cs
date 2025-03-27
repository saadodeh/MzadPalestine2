using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using MzadPalestine.Infrastructure.Data;
using System.Linq.Expressions;

public class NotificationRepository:INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Notification entity)
    {
        throw new NotImplementedException();
    }

    // إضافة إشعار
    public async Task AddNotificationAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public Task AddRangeAsync(IEnumerable<Notification> entities)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(Expression<Func<Notification, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(Expression<Func<Notification, bool>>? predicate = null)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOldNotificationsAsync(int daysOld)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Notification>> FindAsync(Expression<Func<Notification, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<Notification?> FirstOrDefaultAsync(Expression<Func<Notification, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Notification>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Notification>> GetAllWithIncludesAsync(params Expression<Func<Notification, object>>[] includes)
    {
        throw new NotImplementedException();
    }

    public Task<Notification?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Notification?> GetNotificationWithDetailsAsync(int notificationId)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<Notification>> GetPagedAsync(PaginationParams parameters)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<Notification>> GetPagedWithIncludesAsync(PaginationParams parameters, params Expression<Func<Notification, object>>[] includes)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Notification> GetQueryable()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Notification>> GetRecentNotificationsAsync(string userId, int count)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetUnreadNotificationsCountAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<PagedList<Notification>> GetUserNotificationsAsync(string userId, PaginationParams parameters)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasUserEnabledNotificationTypeAsync(string userId, NotificationType type)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsNotificationRecipientAsync(string userId, int notificationId)
    {
        throw new NotImplementedException();
    }

    public Task<Notification?> LastOrDefaultAsync(Expression<Func<Notification, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task MarkAllAsReadAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task MarkAsReadAsync(int notificationId)
    {
        throw new NotImplementedException();
    }

    public void Remove(Notification entity)
    {
        throw new NotImplementedException();
    }

    public void RemoveRange(IEnumerable<Notification> entities)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Notification?> SingleOrDefaultAsync(Expression<Func<Notification, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public void Update(Notification entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserNotificationPreferencesAsync(string userId, IEnumerable<NotificationType> enabledTypes)
    {
        throw new NotImplementedException();
    }

    // استرجاع جميع الإشعارات
    //public async Task<List<Notification>> GetAllNotificationsAsync()
    //{
    //  //  return await _context.Notifications.ToListAsync();
    //}
}
