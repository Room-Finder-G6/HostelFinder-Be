using AutoMapper;
using FluentValidation;
using HostelFinder.Application.DTOs.Image.Requests;
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
        private readonly IUserMembershipRepository _userMembershipRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IValidator<CreateUserRequestDto> _createUserValidator;
        private readonly IS3Service _s3Service;

        public UserService
        (
            IMapper mapper,
            IUserRepository userRepository,
            IMembershipRepository membershipRepository,
            IValidator<CreateUserRequestDto> createUserValidator,
            IS3Service s3Service, 
            IUserMembershipRepository userMembershipRepository
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _createUserValidator = createUserValidator;
            _s3Service = s3Service;
            _membershipRepository = membershipRepository;
            _userMembershipRepository = userMembershipRepository;
        }

        public async Task<Response<UserDto>> RegisterUserAsync(CreateUserRequestDto request)
        {
            var validationResult = await _createUserValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return new Response<UserDto> { Succeeded = false, Errors = errors };
            }

            try
            {
                if (await _userRepository.CheckUserNameExistAsync(request.Username))
                {
                    return new Response<UserDto>
                        { Succeeded = false, Message = "Tên người dùng đã tồn tại. Vui lòng nhập tên khác" };
                }

                if (await _userRepository.CheckEmailExistAsync(request.Email))
                {
                    return new Response<UserDto>
                        { Succeeded = false, Message = "Email đã tồn tại. Vui lòng nhập email khác." };
                }

                if (await _userRepository.CheckPhoneNumberAsync(request.Phone))
                {
                    return new Response<UserDto>
                        { Succeeded = false, Message = "Số điện thoại đã tồn tại. Vui lòng nhập số điện thoại khác." };
                }

                var userDomain = _mapper.Map<User>(request);

                userDomain.Password = _passwordHasher.HashPassword(userDomain, userDomain.Password);
                userDomain.Role = UserRole.User;
                userDomain.IsEmailConfirmed = false;
                userDomain.AvatarUrl =
                    "https://hostel-finder-images.s3.ap-southeast-1.amazonaws.com/Default-Avatar.png";
                userDomain.IsDeleted = false;
                userDomain.CreatedOn = DateTime.Now;

                var user = await _userRepository.AddAsync(userDomain);

                var userDto = _mapper.Map<UserDto>(user);

                return new Response<UserDto>
                    { Succeeded = true, Data = userDto, Message = "Bạn đã đăng ký thành công tài khoản" };
            }
            catch (Exception ex)
            {
                return new Response<UserDto> { Succeeded = false, Errors = { ex.Message } };
            }
        }

        public async Task<Response<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null || !users.Any())
            {
                return new Response<List<UserDto>>
                    { Succeeded = false, Errors = new List<string> { "No users found." } };
            }

            var userDtos = _mapper.Map<List<UserDto>>(users);
            return new Response<List<UserDto>> { Data = userDtos, Succeeded = true };
        }

        public async Task<Response<UserProfileResponse>> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return new Response<UserProfileResponse>
                    { Succeeded = false, Errors = new List<string> { "User not found." } };
            }

            var userProfileResponse = _mapper.Map<UserProfileResponse>(user);

            return new Response<UserProfileResponse> { Data = userProfileResponse, Succeeded = true };
        }

        public async Task<Response<UserDto>> UpdateUserAsync(Guid userId, UpdateUserRequestDto updateUserDto,
            UploadImageRequestDto? image)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new Response<UserDto>("Người dùng không tồn tại.");
            }

            if (image.Image != null)
            {
                var imageUrl = await _s3Service.UploadFileAsync(image.Image);
                user.AvatarUrl = imageUrl;
            }

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;
            user.Phone = updateUserDto.Phone;
            user.FullName = updateUserDto.FullName;

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

        public async Task<Response<UserProfileResponse>> GetUserByHostelIdAsync(Guid hostelId)
        {
            var user = await _userRepository.GetUserByHostelIdAsync(hostelId);
            var userDto = _mapper.Map<UserProfileResponse>(user);
            return new Response<UserProfileResponse>(userDto);
        }

        public async Task<Response<string>> BuyMembershipAsync(Guid userId, Guid membershipId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new Response<string>
                {
                    Succeeded = false,
                    Message = "User not found."
                };
            }

            var userMembership = await _userMembershipRepository.GetByUserIdAsync(userId);

            var membership = await _membershipRepository.GetByIdAsync(membershipId);
            if (membership == null)
            {
                return new Response<string>
                {
                    Succeeded = false,
                    Message = "Membership not found."
                };
            }

            if (user.WalletBalance < membership.Price)
            {
                return new Response<string>
                {
                    Succeeded = false,
                    Message = "Số dư không đủ! Vui lòng nạp tiền thêm vào ví."
                };
            }

            user.WalletBalance -= membership.Price;
            await _userRepository.UpdateAsync(user);

            if (userMembership == null)
            {
                var startDate = DateTime.Now;
                var expiryDate = startDate.AddMonths(membership.Duration);

                var newUserMembership = new UserMembership
                {
                    UserId = userId,
                    MembershipId = membershipId,
                    StartDate = startDate,
                    ExpiryDate = expiryDate,
                    PostsUsed = 0,
                    PushTopUsed = 0,
                    IsPaid = true,
                    CreatedBy = "System",
                    CreatedOn = DateTime.Now
                };

                await _userMembershipRepository.AddAsync(newUserMembership);

                if (user.Role == UserRole.User)
                {
                    user.Role = UserRole.Landlord;
                    await _userRepository.UpdateAsync(user);
                }

                return new Response<string>
                {
                    Succeeded = true,
                    Message = "Mua gói thành viên thành công."
                };
            }

            if (userMembership.MembershipId == membershipId)
            {
                if (userMembership.ExpiryDate > DateTime.Now)
                {
                    return new Response<string>
                    {
                        Succeeded = false,
                        Message = "Bạn đang sử dụng gói thành viên này. Không thể mua thêm!"
                    };
                }
                else
                {
                    userMembership.ExpiryDate = DateTime.Now.AddMonths(membership.Duration); 
                    userMembership.PostsUsed = 0;  
                    userMembership.PushTopUsed = 0;

                    await _userMembershipRepository.UpdateAsync(userMembership);

                    return new Response<string>
                    {
                        Succeeded = true,
                        Message = "Gói thành viên dược gia hạn thành công."
                    };
                }
            }

            if (userMembership.MembershipId != membershipId)
            {
                var startDate = DateTime.Now;
                var expiryDate = startDate.AddMonths(membership.Duration);

                var newUserMembership = new UserMembership
                {
                    UserId = userId,
                    MembershipId = membershipId,
                    StartDate = startDate,
                    ExpiryDate = expiryDate,
                    PostsUsed = 0,
                    PushTopUsed = 0,
                    IsPaid = true,
                    CreatedBy = "System",
                    CreatedOn = DateTime.Now
                };

                await _userMembershipRepository.AddAsync(newUserMembership);

                return new Response<string>
                {
                    Succeeded = true,
                    Message = "Gói thành viên mới đã được thêm thành công."
                };
            }

            return new Response<string>
            {
                Succeeded = false,
                Message = "Không xác định được trạng thái của membership."
            };
        }
    }
}