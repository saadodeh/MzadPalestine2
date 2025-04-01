using MzadPalestine.Core.DTOs.Messages;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces.Services;

public interface IMessageService
{
    Task<Result<MessageDto>> SendMessageAsync(string userId, SendMessageDto messageDto);
    Task<PagedList<MessageDto>> GetInboxMessagesAsync(string userId, PaginationParams parameters);
    Task<PagedList<MessageDto>> GetSentMessagesAsync(string userId, PaginationParams parameters);
    Task<Result<bool>> DeleteMessageAsync(string userId, int messageId);
    Task<Result<bool>> MarkMessageAsReadAsync(string userId, int messageId);
    Task<Result<MessageDto>> GetMessageByIdAsync(string userId, int messageId);
    Task<PagedList<MessageDto>> GetConversationAsync(string userId, string otherUserId, PaginationParams parameters);
}