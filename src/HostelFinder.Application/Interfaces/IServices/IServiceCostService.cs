using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IServiceCostService
    {
        Task<Response<ServiceCostResponseDto>> AddServiceCostAsync(AddServiceCostDto dto);
        Task<Response<ServiceCostResponseDto>> UpdateServiceCostAsync(Guid serviceId, UpdateServiceCostDto dto);
        Task<Response<bool>> DeleteServiceCostAsync(Guid id);
    }
}
