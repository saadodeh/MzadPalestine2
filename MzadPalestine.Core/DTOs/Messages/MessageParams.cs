using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.DTOs.Messages;

public class MessageParams : PaginationParams
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? Unread { get; set; }
}
