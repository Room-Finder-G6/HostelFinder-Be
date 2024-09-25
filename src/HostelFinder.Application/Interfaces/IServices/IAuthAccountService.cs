using HostelFinder.Application.DTOs.Auth.Requests;
using HostelFinder.Application.DTOs.Auth.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IAuthAccountService
    {
        Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);

        Task<Response<string>> ChangePasswordAsync(ChangePasswordRequest request);

    }
}
