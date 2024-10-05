using DocumentFormat.OpenXml.InkML;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class UserRepository : BaseGenericRepository<User>, IUserRepository
    {
        public UserRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if(user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckPhoneNumberAsync(string phoneNumber)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Phone == phoneNumber);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckUserNameExistAsync(string userName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == userName);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var user = await _dbContext.Users.Where(x => x.Email.ToLower().Equals(email.ToLower()) && !x.IsDeleted).FirstOrDefaultAsync();

            if(user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<User?> FindByUserNameAsync(string userName)
        {
            var user = await _dbContext.Users.Where(x => x.Username.ToLower().Equals(userName.ToLower()) && !x.IsDeleted).FirstOrDefaultAsync();
            if (user == null) 
            {
                return null;
            }
            return user;
        }

        public async Task<UserRole> GetRoleAsync(Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user.Role;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
