using AutoMapper;
using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService
        (
            IMapper mapper,
            IUserRepository userRepository
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Response<UserDto>> CreateUserAsync(CreateUserRequestDto request)
        {
            try
            {
                var userDomain = _mapper.Map<User>(request);

                userDomain.IsDeleted = false;
                userDomain.CreatedOn = DateTime.Now;

                var user = await _userRepository.AddAsync(userDomain);

                var userDto = _mapper.Map<UserDto>(user);

                return new Response<UserDto> { Succeeded = true, Data = userDto, Message = "Add user successfully" }; 
            }
            catch (Exception ex)
            {
                return new Response<UserDto> { Succeeded = false, Errors = {ex.Message}};
            }
        }
    }
}
