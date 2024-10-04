using HostelFinder.Application.DTOs.Service.Request;
using HostelFinder.Application.DTOs.Service.Response;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceResponseDTO>> GetAllServicesAsync();
        Task<ServiceResponseDTO?> GetServiceByIdAsync(Guid id);
        Task AddServiceAsync(ServiceCreateRequestDTO serviceCreateRequestDTO);
        Task UpdateServiceAsync(Guid id, ServiceUpdateRequestDTO serviceUpdateRequestDTO);
        Task DeleteServiceAsync(Guid id);
    }
}