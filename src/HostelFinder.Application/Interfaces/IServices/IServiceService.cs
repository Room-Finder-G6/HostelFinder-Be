using HostelFinder.Application.DTOs.Service.Request;
using HostelFinder.Application.DTOs.Service.Response;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IServiceService
    {
        Task<Response<IEnumerable<ServiceResponseDTO>>> GetAllServicesAsync();
        Task<Response<ServiceResponseDTO>> GetServiceByIdAsync(Guid id);
        Task<Response<ServiceResponseDTO>> AddServiceAsync(ServiceCreateRequestDTO serviceCreateRequestDTO);
        Task<Response<ServiceResponseDTO>> UpdateServiceAsync(Guid id, ServiceUpdateRequestDTO serviceUpdateRequestDTO);
        Task<Response<string>> DeleteServiceAsync(Guid id); 
    }
}