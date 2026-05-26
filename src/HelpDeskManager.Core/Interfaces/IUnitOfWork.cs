using HelpDeskManager.Core.Interfaces.Repositories;

namespace HelpDeskManager.Core.Interfaces;

public interface IUnitOfWork
{
    ICustomerRepository CustomerRepository { get; }
    ISupportRequestRepository SupportRequestRepository { get; }
    IUserRepository UserRepository { get; }
    Task<bool> SaveChangesAsync();
    bool HasChanges();
}
