using Google.Apis.Auth;
using HostelFinder.Application.DTOs.Auth.Requests;
using HostelFinder.Application.DTOs.Auth.Responses;
using HostelFinder.Application.DTOs.Auths.Requests;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using HostelFinder.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HostelFinder.Application.Services
{
    public class AuthAccountService : IAuthAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthAccountService(IUserRepository userRepository
            , ITokenService tokenService,
            IEmailService emailService
            )
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<Response<string>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                var user = await _userRepository.FindByUserNameAsync(request.Username);
                if (user == null)
                {
                    return new Response<string> { Succeeded = false, Message = "User not found" };
                }

                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.CurrentPassword);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return new Response<string> { Succeeded = false, Message = "Current password is incorrect" };
                }

                if (request.NewPassword != request.ConfirmPassword)
                {
                    return new Response<string> { Succeeded = false, Message = "New password and confirm password do not match" };
                }

                if (string.Equals(request.CurrentPassword, request.NewPassword))
                {
                    return new Response<string> { Succeeded = false, Message = "New password cannot be the same as the current password" };
                }

                user.Password = _passwordHasher.HashPassword(user, request.NewPassword);
                await _userRepository.UpdateAsync(user);

                return new Response<string> { Succeeded = true, Message = "Password changed successfully" };
            }
            catch (Exception ex)
            {
                return new Response<string> { Succeeded = false, Message = $"Error: {ex.Message}" };
            }
        }


        public async Task<Response<string>> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new Response<string> { Succeeded = false, Message = "Email không tồn tại. Vui lòng kiểm tra hoặc tạo tài khoản mới." };
            }
            var resetToken = await _tokenService.GenerateResetPasswordToken(user);

            var emailBody = EmailConstants.BodyResetPasswordEmail(user.Email, resetToken);
            var emailSubject = "Đặt lại mật khẩu";

            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

            return new Response<string> { Succeeded = true, Message = "Link đặt lại mật khẩu đã được gửi tới email của bạn. Vui lòng check email!" };
        }

        public async Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request)
        {
            try
            {
                var user = await _userRepository.FindByUserNameAsync(request.UserName);
                if (user == null)
                {
                    return new Response<AuthenticationResponse> { Succeeded = false, Message = "Tên người dùng không tồn tại. Vui lòng kiểm tra hoặc tạo tài khoản mới." };
                }

                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return new Response<AuthenticationResponse> { Succeeded = false, Message = "Tài khoản hoặc mật khẩu không đúng. Vui lòng kiểm tra lại!" };
                }

                var role = await _userRepository.GetRoleAsync(user.Id);
                var token = _tokenService.GenerateJwtToken(user, role);

                var response = new AuthenticationResponse
                {
                    UserName = user.Username,
                    Role = role.ToString(),
                    Token = token
                };

                return new Response<AuthenticationResponse> { Data = response, Succeeded = true, Message = "Đăng nhập thành công" };
            }
            catch (Exception ex)
            {
                return new Response<AuthenticationResponse> { Message = ex.Message };
            }
        }

        public async Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new Response<string> { Succeeded = false, Message = "Email không tồn tại. Vui lòng kiểm tra hoặc tạo tài khoản mới." };
            }

            var isValidToken = await _tokenService.ValidateResetPasswordToken(user, request.Token);
            if (!isValidToken)
            {
                return new Response<string> { Succeeded = false, Message = "Token không hợp lệ. Vui lòng check và thử lại" };
            }

            user.Password = _passwordHasher.HashPassword(user, request.NewPassword);

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;

            await _userRepository.UpdateAsync(user);
            return new Response<string> { Succeeded = true, Message = "Đặt lại mật khẩu thành công!" };
        }

        public async Task<Response<AuthenticationResponse>> LoginWithGoogleAsync(LoginWithGoogleRequest request)
        {
            try
            {
                // Logging để kiểm tra idToken
                Console.WriteLine($"Received idToken: {request.IdToken}");

                var googleUser = await AuthenticateWithGoogle(request.IdToken);
                if (googleUser == null)
                    throw new Exception("Invalid Google Token.");

                var user = await _userRepository.FindByEmailAsync(googleUser.Email);
                if (user == null)
                {
                    user = new User
                    {
                        FullName = googleUser.Name,
                        Email = googleUser.Email,
                        AvatarUrl = "https://hostel-finder-images.s3.ap-southeast-1.amazonaws.com/Default-Avatar.png",
                        Role = UserRole.User, 
                        IsActive = true,
                        IsEmailConfirmed = true,
                        WalletBalance = 0,
                        CreatedOn = DateTime.Now,
                        CreatedBy = "Google",
                        Username = googleUser.Name
                    };

                    await _userRepository.AddAsync(user);
                }
                var role = await _userRepository.GetRoleAsync(user.Id);
                var token =  _tokenService.GenerateJwtToken(user, role);
                var response = new AuthenticationResponse
                {
                    UserName = user.FullName,
                    Role = role.ToString(),
                    Token = token
                };
                return new Response<AuthenticationResponse> { Data = response, Succeeded = true, Message = "Đăng nhập thành công" };
                
            }
            catch (Exception ex)
            {
                // Logging lỗi để dễ dàng kiểm tra
                Console.WriteLine($"Error in GoogleLogin: {ex.Message}");
                throw new Exception($"Error: {ex.Message}");
            }
        }
        private async Task<GoogleJsonWebSignature.Payload> AuthenticateWithGoogle(string idToken)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Development.json")
                    .Build();
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { configuration["Google:ClientId"] }
                });
                return payload;
            }
            catch (Exception ex)
            {
                // Logging để kiểm tra lỗi xác thực
                Console.WriteLine($"Invalid JWT: {ex.Message}");
                return null;
            }
        }
    }
}