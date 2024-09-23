using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseGenericRepository<User>
    {
        Task<User> FindByUserNameAsync(string userName);

        Task<User> FindByEmailAsync(string email);

        Task<bool> CheckUserNameExistAsync(string userName);

        Task<bool> CheckEmailExistAsync(string email);

    }
}
