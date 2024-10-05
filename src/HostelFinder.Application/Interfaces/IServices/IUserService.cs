using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<Response<UserDto>> RegisterUserAsync(CreateUserRequestDto request);

        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<Response<UserDto>> UpdateUserAsync(Guid userId, UpdateUserRequestDto updateUserDto);

        Task<Response<bool>> UnActiveUserAsync(Guid userId);
    }
}