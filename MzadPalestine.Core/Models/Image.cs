using MzadPalestine.Core.Models;

public class Image
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int ListingId { get; set; }
    public Listing? Listing { get; set; }
} 