using AutoMapper;
using FluentValidation;
using HostelFinder.Application.DTOs.Users;
using HostelFinder.Application.DTOs.Users.Requests;
using HostelFinder.Application.DTOs.Users.Response;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace HostelFinder.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IValidator<CreateUserRequestDto> _createUserValidator;
        public UserService
        (
            IMapper mapper,
            IUserRepository userRepository,
            IValidator<CreateUserRequestDto> createUserValidator
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _createUserValidator = createUserValidator;
        }

        public async Task<Response<UserDto>> RegisterUserAsync(CreateUserRequestDto request)
        {
            var validationResult = await _createUserValidator.ValidateAsync( request );
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return new Response<UserDto> {Succeeded = false, Errors = errors };
            }
            try
            {
                if (await _userRepository.CheckUserNameExistAsync(request.Username))
                {
                    return new Response<UserDto> { Succeeded = false, Message = "Tên người dùng đã tồn tại. Vui lòng nhập tên khác" };
                }
                if (await _userRepository.CheckEmailExistAsync(request.Email))
                {
                    return new Response<UserDto> { Succeeded = false, Message = "Email đã tồn tại. Vui lòng nhập email khác." };
                }
                if (await _userRepository.CheckPhoneNumberAsync(request.Phone))
                {
                    return new Response<UserDto> { Succeeded = false, Message = "Số điện thoại đã tồn tại. Vui lòng nhập số điện thoại khác." };
                }

                var userDomain = _mapper.Map<User>(request);

                userDomain.Password = _passwordHasher.HashPassword(userDomain, userDomain.Password);
                userDomain.Role = UserRole.User;
                userDomain.IsEmailConfirmed = false;
                userDomain.IsDeleted = false;
                userDomain.CreatedOn = DateTime.Now;

                var user = await _userRepository.AddAsync(userDomain);

                var userDto = _mapper.Map<UserDto>(user);

                return new Response<UserDto> { Succeeded = true, Data = userDto, Message = "Bạn đã đăng ký thành công tài khoản" };
            }
            catch (Exception ex)
            {
                return new Response<UserDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserProfileResponse> GetUserByIdAsync(Guid id)
        {
            var users = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserProfileResponse>(users);
        }

        public async Task<Response<UserDto>> UpdateUserAsync(Guid userId, UpdateUserRequestDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new Response<UserDto>("Người dùng không tồn tại.");
            }

            // Update fields
            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;
            user.Phone = updateUserDto.Phone;
            user.AvatarUrl = updateUserDto.AvatarUrl;

            await _userRepository.UpdateAsync(user);
            var updatedUserDto = _mapper.Map<UserDto>(user);
            return new Response<UserDto>(updatedUserDto);
        }

        public async Task<Response<bool>> UnActiveUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new Response<bool>("Người dùng không tồn tại.");
            }

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
            return new Response<bool>(true);
        }
    }
}