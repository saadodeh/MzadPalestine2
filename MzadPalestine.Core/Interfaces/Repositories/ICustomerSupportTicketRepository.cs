using MzadPalestine.Core.Models;

namespace MzadPalestine.Core.Interfaces.Repositories;

public interface ICustomerSupportTicketRepository : IGenericRepository<CustomerSupportTicket>
{
    Task<IEnumerable<CustomerSupportTicket>> GetUserTicketsAsync(string userId);
    Task<IEnumerable<CustomerSupportTicket>> GetOpenTicketsAsync();
    Task<IEnumerable<CustomerSupportTicket>> GetClosedTicketsAsync();
}
