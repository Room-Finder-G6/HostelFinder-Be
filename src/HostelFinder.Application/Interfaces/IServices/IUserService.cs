using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<Response<UserDto>> CreateUserAsync(CreateUserRequestDto request);
    }
}
