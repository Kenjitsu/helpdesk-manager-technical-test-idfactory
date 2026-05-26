using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.Core.Interfaces.Repositories;
using HelpDeskManager.DAL.Data;
using HelpDeskManager.DAL.Data.Identity;
using HelpDeskManager.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskManager.DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UnitOfWork(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    private ICustomerRepository? _customerRepository;
    private ISupportRequestRepository? _supportRequestRepository;
    private IUserRepository? _userRepository;

    public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_context);

    public ISupportRequestRepository SupportRequestRepository => _supportRequestRepository ??= new SupportRequestRepository(_context);
    
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_userManager, _context);


    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An exception ocurred while saving changes.", ex);
        }
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}
