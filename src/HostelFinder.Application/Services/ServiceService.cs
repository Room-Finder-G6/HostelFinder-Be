using AutoMapper;
using HostelFinder.Application.DTOs.Service.Request;
using HostelFinder.Application.DTOs.Service.Response;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceResponseDTO>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.ListAllAsync();
            return _mapper.Map<IEnumerable<ServiceResponseDTO>>(services);
        }

        public async Task<ServiceResponseDTO?> GetServiceByIdAsync(Guid id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            return _mapper.Map<ServiceResponseDTO>(service);
        }

        public async Task AddServiceAsync(ServiceCreateRequestDTO serviceCreateRequestDTO)
        {
            var service = _mapper.Map<Service>(serviceCreateRequestDTO);
            await _serviceRepository.AddAsync(service);
        }

        public async Task UpdateServiceAsync(Guid id, ServiceUpdateRequestDTO serviceUpdateRequestDTO)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service != null)
            {
                _mapper.Map(serviceUpdateRequestDTO, service);
                await _serviceRepository.UpdateAsync(service);
            }
        }

        public async Task DeleteServiceAsync(Guid id)
        {
            await _serviceRepository.DeletePermanentAsync(id);
        }
    }
}
