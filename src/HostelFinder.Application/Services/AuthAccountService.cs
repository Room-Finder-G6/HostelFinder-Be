﻿using HostelFinder.Application.DTOs.Auth.Requests;
using HostelFinder.Application.DTOs.Auth.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Services
{
    public class AuthAccountService : IAuthAccountService
    {
        public Task<Response<string>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}