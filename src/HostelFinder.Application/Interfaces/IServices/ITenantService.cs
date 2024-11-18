using HostelFinder.Application.DTOs.RentalContract.Request;
using HostelFinder.Application.DTOs.RentalContract.Response;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface ITenantService
    {
        Task<TenantResponse> AddTenentServiceAsync(AddTenantDto request);
    }
}
