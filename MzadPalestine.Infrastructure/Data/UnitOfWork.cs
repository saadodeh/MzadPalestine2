using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Repositories;
using MzadPalestine.Infrastructure.Services;

namespace MzadPalestine.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IUserConnectionManager _userConnectionManager;
    private Core.Interfaces.Repositories.IAuctionRepository _auctions = null!;
    private Core.Interfaces.Repositories.IBidRepository _bids = null!;
    private ICustomerSupportTicketRepository _customerSupportTickets = null!;
    private Core.Interfaces.Repositories.IDisputeRepository _disputes = null!;
    private Core.Interfaces.Repositories.IInvoiceRepository _invoices = null!;
    private Core.Interfaces.Repositories.IListingRepository _listings = null!;
    private Core.Interfaces.Repositories.INotificationRepository _notifications = null!;
    private Core.Interfaces.Repositories.IReviewRepository _reviews = null!;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IWalletRepository _walletRepository;

    public UnitOfWork(
        ApplicationDbContext context,
        IUserConnectionManager userConnectionManager,
        ISubscriptionRepository subscriptionRepository,
        ITagRepository tagRepository,
        IWalletRepository walletRepository)
    {
        _context = context;
        _userConnectionManager = userConnectionManager;
        _subscriptionRepository = subscriptionRepository;
        _tagRepository = tagRepository;
        _walletRepository = walletRepository;
    }

    public Core.Interfaces.Repositories.IAuctionRepository Auctions => _auctions ??= new AuctionRepository(_context);
    public Core.Interfaces.Repositories.IBidRepository Bids => _bids ??= new BidRepository(_context);
    //public ICustomerSupportTicketRepository CustomerSupportTickets => _customerSupportTickets ??= new CustomerSupportTicketRepository(_context);
    public Core.Interfaces.Repositories.IDisputeRepository Disputes => _disputes ??= new DisputeRepository(_context);
    public Core.Interfaces.Repositories.IInvoiceRepository Invoices => _invoices ??= new InvoiceRepository(_context);
    public Core.Interfaces.Repositories.IListingRepository Listings => _listings ??= new ListingRepository(_context);
    //public Core.Interfaces.Repositories.INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);
    public Core.Interfaces.Repositories.IReviewRepository Reviews => _reviews ??= new ReviewRepository(_context);
    public ISubscriptionRepository SubscriptionRepository => _subscriptionRepository;
    public ITagRepository TagRepository => _tagRepository;
    public IWalletRepository WalletRepository => _walletRepository;
    public IUserConnectionManager UserConnectionManager => _userConnectionManager;

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
