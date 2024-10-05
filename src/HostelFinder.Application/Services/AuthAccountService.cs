using HostelFinder.Application.DTOs.Auth.Requests;
using HostelFinder.Application.DTOs.Auth.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HostelFinder.Application.Services
{
    public class AuthAccountService : IAuthAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher;
        public AuthAccountService(IUserRepository userRepository
            , ITokenService tokenService
            )
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public Task<Response<string>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request)
        {
            var user = await _userRepository.FindByUserNameAsync(request.UserName);
            if (user == null)
            {
                return new Response<AuthenticationResponse> { Succeeded = false, Message = "User name does not exist. Please check and try again or create a new account." };
            }

            var verificationResult =  _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return new Response<AuthenticationResponse> { Succeeded = false, Message = "Incorrect username or password. Please check your credentials and try again." };
            }

            var role = await _userRepository.GetRoleAsync(user.Id);
            var token = _tokenService.GenerateJwtToken(user, role);

            var response = new AuthenticationResponse
            {
                UserName = user.Username,
                Role = role.ToString(),
                Token = token
            };

            return new Response<AuthenticationResponse> { Data = response, Succeeded = true, Message = "Login successfully!" };
        }
    }
}
