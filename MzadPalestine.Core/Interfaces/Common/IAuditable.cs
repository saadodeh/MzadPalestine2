namespace MzadPalestine.Core.Interfaces.Common;

public interface IAuditable
{
    string? CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }
    string? LastModifiedBy { get; set; }
    DateTime? LastModifiedAt { get; set; }
}
