using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<AutoBidRepository> _logger;
    private Core.Interfaces.Repositories.IAuctionRepository? _auctions;
    private Core.Interfaces.IBidRepository _bids = null!;
    private IAutoBidRepository _autoBids = null!;
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
        ApplicationDbContext context ,
        IUserConnectionManager userConnectionManager ,
        ISubscriptionRepository subscriptionRepository ,
        ITagRepository tagRepository ,
        IWalletRepository walletRepository ,
        ILogger<AutoBidRepository> logger)
    {
        _context = context;
        _userConnectionManager = userConnectionManager;
        _subscriptionRepository = subscriptionRepository;
        _tagRepository = tagRepository;
        _walletRepository = walletRepository;
        _logger = logger;
    }

    public Core.Interfaces.Repositories.IAuctionRepository Auctions
    {
        get
        {
            _auctions ??= new AuctionRepository(_context) as Core.Interfaces.Repositories.IAuctionRepository;
            return _auctions;
        }
    }


    public IAutoBidRepository AutoBids
    {
        get
        {
            if (_autoBids == null)
                _autoBids = new AutoBidRepository(_context , _logger);
            return _autoBids;
        }
    }

    public ICustomerSupportTicketRepository CustomerSupportTickets
    {
        get
        {
            if (_customerSupportTickets == null)
                _customerSupportTickets = new CustomerSupportTicketRepository(_context);
            return _customerSupportTickets;
        }
    }

    public Core.Interfaces.Repositories.IDisputeRepository Disputes
    {
        get
        {
            if (_disputes == null)
                _disputes = new DisputeRepository(_context);
            return _disputes;
        }
    }

    public Core.Interfaces.Repositories.IInvoiceRepository Invoices
    {
        get
        {
            if (_invoices == null)
                _invoices = new InvoiceRepository(_context);
            return _invoices;
        }
    }

    public Core.Interfaces.Repositories.IListingRepository Listings
    {
        get
        {
            if (_listings == null)
                _listings = new ListingRepository(_context);
            return _listings;
        }
    }

    //public Core.Interfaces.Repositories.INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);

    public Core.Interfaces.Repositories.IReviewRepository Reviews
    {
        get
        {
            if (_reviews == null)
                _reviews = new ReviewRepository(_context);
            return _reviews;
        }
    }
    public ISubscriptionRepository SubscriptionRepository => _subscriptionRepository;
    public ITagRepository TagRepository => _tagRepository;
    public IWalletRepository WalletRepository => _walletRepository;
    public IUserConnectionManager UserConnectionManager => _userConnectionManager;

    Core.Interfaces.IDisputeRepository IUnitOfWork.Disputes => throw new NotImplementedException();

    public IDisputeEvidenceRepository DisputeEvidences => throw new NotImplementedException();

    Core.Interfaces.IAuctionRepository IUnitOfWork.Auctions => throw new NotImplementedException();

    public Core.Interfaces.IBidRepository Bids
    {
        get
        {
            _bids ??= new BidRepository(_context);
            return _bids;
        }
    }

    public async Task<int> SaveChangesAsync( )
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync( )
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync( )
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync( )
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public void Dispose( )
    {
        _context.Dispose();
    }
}
