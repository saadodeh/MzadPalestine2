using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IMessageRepository : IGenericRepository<Message>
{
    Task<PagedList<Message>> GetUserInboxAsync(string userId, PaginationParams parameters);
    Task<PagedList<Message>> GetUserSentMessagesAsync(string userId, PaginationParams parameters);
    Task<PagedList<Message>> GetConversationAsync(string userId1, string userId2, PaginationParams parameters);
    Task<Message?> GetMessageWithDetailsAsync(int messageId);
    Task<bool> IsMessageRecipientAsync(string userId, int messageId);
    Task<bool> IsMessageSenderAsync(string userId, int messageId);
    Task MarkMessageAsReadAsync(int messageId);
    Task MarkAllMessagesAsReadAsync(string userId);
    Task<int> GetUnreadMessagesCountAsync(string userId);
    Task<IEnumerable<string>> GetUserConversationPartnersAsync(string userId);
}
